import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {Button} from "@/components/ui/button.tsx";
import {Input} from "@/components/ui/input.tsx";
import {Select, SelectContent, SelectItem, SelectTrigger, SelectValue} from "@/components/ui/select.tsx";
import {Form, FormControl, FormField, FormItem, FormLabel, FormMessage} from "@/components/ui/form.tsx";
import {useFieldArray, useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {z} from "zod";
import {ConnectionDetailsDto, DayOfWeek, UpdateScheduleCommand} from "@/schemas/connection-schema.ts";
import {useEffect, useState} from "react";
import {fetchStationsList, SystemListItemDto} from "@/api/system-lists-api.ts";
import {Checkbox} from "@/components/ui/checkbox.tsx";
import {PlusIcon, TrashIcon} from "lucide-react";
import {toast} from "sonner";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";

// Days of week options
const daysOfWeekOptions = [
  {value: DayOfWeek.Monday, label: "Monday"},
  {value: DayOfWeek.Tuesday, label: "Tuesday"},
  {value: DayOfWeek.Wednesday, label: "Wednesday"},
  {value: DayOfWeek.Thursday, label: "Thursday"},
  {value: DayOfWeek.Friday, label: "Friday"},
  {value: DayOfWeek.Saturday, label: "Saturday"},
  {value: DayOfWeek.Sunday, label: "Sunday"},
];

// Schema for the form
const scheduleFormSchema = z.object({
  validFrom: z.string().min(1, {message: "Valid from date is required"}),
  validTo: z.string().min(1, {message: "Valid to date is required"}),
  daysOfWeek: z.array(z.nativeEnum(DayOfWeek)).min(1, {message: "At least one day of week must be selected"}),
  stations: z.array(
    z.object({
      stationId: z.string().uuid({message: "Station is required"}),
      arrivalTime: z.string().nullable(),
      departureTime: z.string().nullable(),
      sequenceNumber: z.number(),
    })
  ).min(2, {message: "At least two stations are required"}),
});

type ScheduleFormValues = z.infer<typeof scheduleFormSchema>;

type EditScheduleFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  operatorId: string;
  connection: ConnectionDetailsDto | null;
  onSave: (command: UpdateScheduleCommand) => void;
};

export function EditScheduleForm(props: EditScheduleFormProps) {
  const {open, setOpen, connection, onSave} = props;

  const [loading, setLoading] = useState(false);
  const [stations, setStations] = useState<SystemListItemDto[]>([]);

  // Fetch stations list
  useEffect(() => {
    if (open) {
      fetchStationsList()
        .then(data => setStations(data))
        .catch(error => {
          console.error("Error fetching stations:", error);
          toast.error("Failed to load stations");
        });
    }
  }, [open]);

  // Initialize form with connection data if available
  // Log the incoming daysOfWeek to debug
  console.log("Incoming daysOfWeek:", connection?.daysOfWeek);

  // Convert daysOfWeek to DayOfWeek enum values if they're strings
  const convertedDaysOfWeek = connection?.daysOfWeek?.map(day => {
    // If day is already a number, return it
    if (typeof day === 'number') {
      return day;
    }
    // If day is a string, convert it to the corresponding DayOfWeek enum value
    if (typeof day === 'string') {
      const dayMap: Record<string, DayOfWeek> = {
        "Sunday": DayOfWeek.Sunday,
        "Monday": DayOfWeek.Monday,
        "Tuesday": DayOfWeek.Tuesday,
        "Wednesday": DayOfWeek.Wednesday,
        "Thursday": DayOfWeek.Thursday,
        "Friday": DayOfWeek.Friday,
        "Saturday": DayOfWeek.Saturday,
        "0": DayOfWeek.Sunday,
        "1": DayOfWeek.Monday,
        "2": DayOfWeek.Tuesday,
        "3": DayOfWeek.Wednesday,
        "4": DayOfWeek.Thursday,
        "5": DayOfWeek.Friday,
        "6": DayOfWeek.Saturday
      };
      return dayMap[day] !== undefined ? dayMap[day] : null;
    }
    return null;
  }).filter(day => day !== null) || [];

  console.log("Converted daysOfWeek:", convertedDaysOfWeek);

  const form = useForm<ScheduleFormValues>({
    resolver: zodResolver(scheduleFormSchema),
    defaultValues: {
      validFrom: connection?.validFrom || new Date().toISOString().split('T')[0],
      validTo: connection?.validTo || new Date(new Date().setFullYear(new Date().getFullYear() + 1)).toISOString().split('T')[0],
      daysOfWeek: convertedDaysOfWeek,
      stations: connection?.stations.map(station => ({
        stationId: station.stationId,
        arrivalTime: station.arrivalTime,
        departureTime: station.departureTime,
        sequenceNumber: station.sequenceNumber,
      })) || [],
    },
  });

  // Use field array for dynamic stations list
  const {fields, append, remove, update} = useFieldArray({
    control: form.control,
    name: "stations",
  });

  // Add a new station to the list
  const addStation = () => {
    append({
      stationId: "",
      arrivalTime: null,
      departureTime: null,
      sequenceNumber: fields.length + 1,
    });
  };

  // Initialize with at least two stations if none exist
  useEffect(() => {
    if (fields.length === 0 && open) {
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
  }, [fields.length, append, open]);

  // Handle form submission
  const onSubmit = async (values: ScheduleFormValues) => {
    setLoading(true);
    try {
      // Ensure first station has no arrival time and last station has no departure time
      const processedStations = values.stations.map((station, index, array) => {
        if (index === 0) {
          return {...station, arrivalTime: null};
        }
        if (index === array.length - 1) {
          return {...station, departureTime: null};
        }
        return station;
      });

      const command: UpdateScheduleCommand = {
        id: connection?.id || "",
        schedule: {
          validFrom: values.validFrom,
          validTo: values.validTo,
          daysOfWeek: values.daysOfWeek,
        },
        connectionStations: processedStations,
      };

      await onSave(command);
      setOpen(false);
    } catch (error) {
      console.error("Error saving schedule:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className="max-w-6xl max-h-[80vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Edit Connection Schedule</DialogTitle>
          <DialogDescription>
            Update the schedule and stations for this connection.
          </DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            {/* Schedule Section */}
            <div className="space-y-4">
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
                                        ? field.onChange([...field.value, option.value])
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
            <div className="space-y-4">
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

              {form.formState.errors.stations && (
                <p className="text-sm font-medium text-destructive">{form.formState.errors.stations.message}</p>
              )}
            </div>

            <div className="flex justify-end gap-2 pt-4">
              <Button type="button" variant="outline" onClick={() => setOpen(false)}>
                Cancel
              </Button>
              <Button type="submit" disabled={loading}>
                {loading ? "Saving..." : "Save Schedule"}
              </Button>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}
