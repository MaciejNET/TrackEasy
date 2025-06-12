import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {Button} from "@/components/ui/button.tsx";
import {Input} from "@/components/ui/input.tsx";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/ui/select.tsx";
import {Form, FormControl, FormField, FormItem, FormLabel, FormMessage} from "@/components/ui/form.tsx";
import {useFieldArray, useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {z} from "zod";
import {
  ConnectionDetailsDto,
  CreateConnectionCommand,
  Currency,
  UpdateConnectionCommand
} from "@/schemas/connection-schema.ts";
import {useEffect, useState} from "react";
import {fetchStationsList, SystemListItemDto} from "@/api/system-lists-api.ts";
import {Checkbox} from "@/components/ui/checkbox.tsx";
import {PlusIcon, TrashIcon} from "lucide-react";
import {toast} from "sonner";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";

// Mock train data - in a real app, this would come from an API
const mockTrains = [
  {id: "3fa85f64-5717-4562-b3fc-2c963f66afa6", name: "Train 1"},
  {id: "4fa85f64-5717-4562-b3fc-2c963f66afa6", name: "Train 2"},
  {id: "5fa85f64-5717-4562-b3fc-2c963f66afa6", name: "Train 3"},
];

// Days of week options
const daysOfWeekOptions = [
  {value: "Monday", label: "Monday"},
  {value: "Tuesday", label: "Tuesday"},
  {value: "Wednesday", label: "Wednesday"},
  {value: "Thursday", label: "Thursday"},
  {value: "Friday", label: "Friday"},
  {value: "Saturday", label: "Saturday"},
  {value: "Sunday", label: "Sunday"},
];

const connectionFormSchema = z.object({
  name: z.string().min(3, {message: 'Name must be at least 3 characters'}).max(100, {message: 'Name must be at most 100 characters'}),
  priceAmount: z.coerce.number().min(0.01, {message: 'Price must be greater than 0'}),
  currency: z.nativeEnum(Currency),
  trainId: z.string().uuid({message: 'Please select a train'}),
  needsSeatReservation: z.boolean().default(false),
  // Schedule fields (only used when creating a new connection)
  validFrom: z.string().optional(),
  validTo: z.string().optional(),
  daysOfWeek: z.array(z.string()).optional(),
  // Stations (only used when creating a new connection)
  stations: z.array(
    z.object({
      stationId: z.string().uuid({message: "Station is required"}),
      arrivalTime: z.string().nullable(),
      departureTime: z.string().nullable(),
      sequenceNumber: z.number(),
    })
  ).optional(),
});

type ConnectionFormValues = z.infer<typeof connectionFormSchema>;

type AddEditConnectionFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  operatorId: string;
  connection?: ConnectionDetailsDto;
  onSave: (command: CreateConnectionCommand | UpdateConnectionCommand) => void;
};

export function AddEditConnectionForm(props: AddEditConnectionFormProps) {
  const {open, setOpen, operatorId, connection, onSave} = props;
  const isEditing = !!connection;

  const [loading, setLoading] = useState(false);
  const [stations, setStations] = useState<SystemListItemDto[]>([]);

  // Fetch stations list
  useEffect(() => {
    if (open && !isEditing) {
      fetchStationsList()
        .then(data => setStations(data))
        .catch(error => {
          console.error("Error fetching stations:", error);
          toast.error("Failed to load stations");
        });
    }
  }, [open, isEditing]);

  const form = useForm<ConnectionFormValues>({
    resolver: zodResolver(connectionFormSchema),
    defaultValues: {
      name: connection?.name || "",
      priceAmount: connection?.pricePerKilometer.amount || 0,
      currency: connection?.pricePerKilometer.currency || Currency.PLN,
      trainId: connection?.trainId || "",
      needsSeatReservation: connection?.needsSeatReservation || false,
      validFrom: new Date().toISOString().split('T')[0],
      validTo: new Date(new Date().setFullYear(new Date().getFullYear() + 1)).toISOString().split('T')[0],
      daysOfWeek: ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"],
      stations: [],
    },
  });

  // Use field array for dynamic stations list (only for new connections)
  const {fields, append, remove, update} = useFieldArray({
    control: form.control,
    name: "stations",
  });

  // Initialize with at least two stations if none exist and we're creating a new connection
  useEffect(() => {
    if (fields.length === 0 && open && !isEditing) {
      // Add two empty stations
      append([
        {
          stationId: "",
          arrivalTime: null,
          departureTime: null,
          sequenceNumber: 1,
        },
        {
          stationId: "",
          arrivalTime: null,
          departureTime: null,
          sequenceNumber: 2,
        }
      ]);
    }
  }, [fields.length, append, open, isEditing]);

  // Add a new station to the list
  const addStation = () => {
    append({
      stationId: "",
      arrivalTime: null,
      departureTime: null,
      sequenceNumber: fields.length + 1,
    });
  };

  const onSubmit = async (values: ConnectionFormValues) => {
    setLoading(true);
    try {
      if (isEditing) {
        // Update existing connection
        const command: UpdateConnectionCommand = {
          id: connection.id,
          name: values.name,
          money: {
            amount: values.priceAmount,
            currency: values.currency
          }
        };
        await onSave(command);
      } else {
        // Create new connection with schedule and stations
        // Process stations to ensure first station has no arrival time and last station has no departure time
        const processedStations = values.stations?.map((station, index, array) => {
          if (index === 0) {
            return {...station, arrivalTime: null};
          }
          if (index === array.length - 1) {
            return {...station, departureTime: null};
          }
          return station;
        }) || [];

        const command: CreateConnectionCommand = {
          name: values.name,
          operatorId: operatorId,
          pricePerKilometer: {
            amount: values.priceAmount,
            currency: values.currency
          },
          trainId: values.trainId,
          needsSeatReservation: values.needsSeatReservation,
          schedule: {
            validFrom: values.validFrom || new Date().toISOString().split('T')[0],
            validTo: values.validTo || new Date(new Date().setFullYear(new Date().getFullYear() + 1)).toISOString().split('T')[0],
            daysOfWeek: values.daysOfWeek || ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"]
          },
          connectionStations: processedStations
        };
        await onSave(command);
      }
      setOpen(false);
    } catch (error) {
      console.error("Error saving connection:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className={isEditing ? "sm:max-w-[600px] max-h-[80vh] overflow-y-auto" : "max-w-6xl max-h-[80vh] overflow-y-auto"}>
        <DialogHeader>
          <DialogTitle>{isEditing ? "Edit Connection" : "Add Connection"}</DialogTitle>
          <DialogDescription>
            {isEditing
              ? "Update the connection details below."
              : "Fill in the details to create a new connection."}
          </DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <FormField
              control={form.control}
              name="name"
              render={({field}) => (
                <FormItem>
                  <FormLabel>Name</FormLabel>
                  <FormControl>
                    <Input placeholder="Connection name" {...field} />
                  </FormControl>
                  <FormMessage/>
                </FormItem>
              )}
            />

            <div className="grid grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="priceAmount"
                render={({field}) => (
                  <FormItem>
                    <FormLabel>Price per Kilometer</FormLabel>
                    <FormControl>
                      <Input type="number" step="0.01" {...field} />
                    </FormControl>
                    <FormMessage/>
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="currency"
                render={({field}) => (
                  <FormItem>
                    <FormLabel>Currency</FormLabel>
                    <Select
                      onValueChange={field.onChange}
                      defaultValue={field.value}
                    >
                      <FormControl>
                        <SelectTrigger>
                          <SelectValue placeholder="Select currency"/>
                        </SelectTrigger>
                      </FormControl>
                      <SelectContent>
                        {Object.values(Currency).map((currency) => (
                          <SelectItem key={currency} value={currency}>
                            {currency}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                    <FormMessage/>
                  </FormItem>
                )}
              />
            </div>

            {!isEditing && (
              <>
                <FormField
                  control={form.control}
                  name="trainId"
                  render={({field}) => (
                    <FormItem>
                      <FormLabel>Train</FormLabel>
                      <Select
                        onValueChange={field.onChange}
                        defaultValue={field.value}
                      >
                        <FormControl>
                          <SelectTrigger>
                            <SelectValue placeholder="Select a train"/>
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent>
                          {mockTrains.map((train) => (
                            <SelectItem key={train.id} value={train.id}>
                              {train.name}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                      <FormMessage/>
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="needsSeatReservation"
                  render={({field}) => (
                    <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-4">
                      <FormControl>
                        <input
                          type="checkbox"
                          checked={field.value}
                          onChange={field.onChange}
                          className="h-4 w-4 rounded border-gray-300 text-indigo-600 focus:ring-indigo-500"
                        />
                      </FormControl>
                      <div className="space-y-1 leading-none">
                        <FormLabel>Requires Seat Reservation</FormLabel>
                        <p className="text-sm text-gray-500">
                          Check this if passengers must select seats when booking
                        </p>
                      </div>
                    </FormItem>
                  )}
                />

                {/* Schedule Section */}
                <div className="space-y-4 mt-6">
                  <h3 className="text-lg font-medium">Schedule</h3>
                  <div className="grid grid-cols-2 gap-4">
                    <FormField
                      control={form.control}
                      name="validFrom"
                      render={({field}) => (
                        <FormItem>
                          <FormLabel>Valid From</FormLabel>
                          <FormControl>
                            <Input type="date" {...field} />
                          </FormControl>
                          <FormMessage/>
                        </FormItem>
                      )}
                    />

                    <FormField
                      control={form.control}
                      name="validTo"
                      render={({field}) => (
                        <FormItem>
                          <FormLabel>Valid To</FormLabel>
                          <FormControl>
                            <Input type="date" {...field} />
                          </FormControl>
                          <FormMessage/>
                        </FormItem>
                      )}
                    />
                  </div>

                  <FormField
                    control={form.control}
                    name="daysOfWeek"
                    render={() => (
                      <FormItem>
                        <div className="mb-4">
                          <FormLabel className="text-base">Days of Week</FormLabel>
                        </div>
                        <div className="grid grid-cols-4 gap-2">
                          {daysOfWeekOptions.map((option) => (
                            <FormField
                              key={option.value}
                              control={form.control}
                              name="daysOfWeek"
                              render={({field}) => {
                                return (
                                  <FormItem
                                    key={option.value}
                                    className="flex flex-row items-start space-x-3 space-y-0"
                                  >
                                    <FormControl>
                                      <Checkbox
                                        checked={field.value?.includes(option.value)}
                                        onCheckedChange={(checked) => {
                                          return checked
                                            ? field.onChange([...(field.value || []), option.value])
                                            : field.onChange(
                                              field.value?.filter(
                                                (value) => value !== option.value
                                              )
                                            )
                                        }}
                                      />
                                    </FormControl>
                                    <FormLabel className="font-normal">
                                      {option.label}
                                    </FormLabel>
                                  </FormItem>
                                )
                              }}
                            />
                          ))}
                        </div>
                        <FormMessage/>
                      </FormItem>
                    )}
                  />
                </div>

                {/* Stations Section */}
                <div className="space-y-4 mt-6">
                  <div className="flex justify-between items-center">
                    <h3 className="text-lg font-medium">Stations</h3>
                    <Button
                      type="button"
                      variant="outline"
                      size="sm"
                      onClick={addStation}
                    >
                      <PlusIcon className="h-4 w-4 mr-2"/>
                      Add Station
                    </Button>
                  </div>

                  {fields.length > 0 ? (
                    <div className="border rounded-md overflow-hidden">
                      <Table>
                        <TableHeader>
                          <TableRow>
                            <TableHead>Sequence</TableHead>
                            <TableHead>Station</TableHead>
                            <TableHead>Arrival Time</TableHead>
                            <TableHead>Departure Time</TableHead>
                            <TableHead>Actions</TableHead>
                          </TableRow>
                        </TableHeader>
                        <TableBody>
                          {fields.map((field, index) => (
                            <TableRow key={field.id}>
                              <TableCell>{index + 1}</TableCell>
                              <TableCell>
                                <FormField
                                  control={form.control}
                                  name={`stations.${index}.stationId`}
                                  render={({field}) => (
                                    <FormItem>
                                      <Select
                                        onValueChange={field.onChange}
                                        value={field.value}
                                      >
                                        <FormControl>
                                          <SelectTrigger className="w-full">
                                            <SelectValue placeholder="Select a station"/>
                                          </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                          {stations.map((station) => (
                                            <SelectItem key={station.id} value={station.id}>
                                              {station.name}
                                            </SelectItem>
                                          ))}
                                        </SelectContent>
                                      </Select>
                                      <FormMessage/>
                                    </FormItem>
                                  )}
                                />
                              </TableCell>
                              <TableCell>
                                <FormField
                                  control={form.control}
                                  name={`stations.${index}.arrivalTime`}
                                  render={({field}) => (
                                    <FormItem>
                                      <FormControl>
                                        <Input
                                          type="time"
                                          {...field}
                                          value={field.value || ""}
                                          onChange={(e) => field.onChange(e.target.value || null)}
                                          disabled={index === 0} // Disable for first station
                                        />
                                      </FormControl>
                                      <FormMessage/>
                                    </FormItem>
                                  )}
                                />
                              </TableCell>
                              <TableCell>
                                <FormField
                                  control={form.control}
                                  name={`stations.${index}.departureTime`}
                                  render={({field}) => (
                                    <FormItem>
                                      <FormControl>
                                        <Input
                                          type="time"
                                          {...field}
                                          value={field.value || ""}
                                          onChange={(e) => field.onChange(e.target.value || null)}
                                          disabled={index === fields.length - 1} // Disable for last station
                                        />
                                      </FormControl>
                                      <FormMessage/>
                                    </FormItem>
                                  )}
                                />
                              </TableCell>
                              <TableCell>
                                <Button
                                  type="button"
                                  variant="ghost"
                                  size="sm"
                                  onClick={() => {
                                    if (fields.length > 2) {
                                      remove(index);
                                      // Update sequence numbers
                                      fields.forEach((f, i) => {
                                        if (i >= index) {
                                          update(i, {
                                            ...f,
                                            sequenceNumber: i + 1
                                          });
                                        }
                                      });
                                    } else {
                                      toast.error("At least two stations are required");
                                    }
                                  }}
                                  disabled={fields.length <= 2}
                                >
                                  <TrashIcon className="h-4 w-4"/>
                                </Button>
                              </TableCell>
                            </TableRow>
                          ))}
                        </TableBody>
                      </Table>
                    </div>
                  ) : (
                    <div className="text-center p-4 border rounded-md">
                      <p className="text-gray-500">No stations added yet. Click "Add Station" to add stations.</p>
                    </div>
                  )}
                </div>
              </>
            )}

            <div className="flex justify-end gap-2 pt-4">
              <Button type="button" variant="outline" onClick={() => setOpen(false)}>
                Cancel
              </Button>
              <Button type="submit" disabled={loading}>
                {loading ? "Saving..." : "Save"}
              </Button>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}
