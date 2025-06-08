import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {ModalType} from "@/types/modals.ts";
import {useEffect, useState} from "react";
import {
  CoachDto,
  CoachDetailsDto,
  CreateCoachCommand,
  createCoachCommandSchema,
  UpdateCoachCommand,
  updateCoachCommandSchema
} from "@/schemas/coach-schema.ts";
import {useQuery} from "@tanstack/react-query";
import {fetchCoach} from "@/api/coaches-api.ts";
import {Loader} from "@/components/loader.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";
import {X} from "lucide-react";

type AddEditCoachFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  handleSave: (coach: CreateCoachCommand | UpdateCoachCommand) => void;
  modalType: ModalType | null;
  coach: CoachDto | null;
  operatorId: string;
};

export function AddEditCoachForm(props: AddEditCoachFormProps) {
  const {open, setOpen, handleSave, modalType, coach, operatorId} = props;
  const [seatNumber, setSeatNumber] = useState<string>("");
  const [seatsNumbers, setSeatsNumbers] = useState<number[]>([]);

  // Fetch coach details if editing
  const {
    data: coachDetails,
    isLoading,
    isError
  } = useQuery({
    queryKey: ['coach-details-for-edit', operatorId, coach?.id],
    queryFn: () => coach ? fetchCoach(operatorId, coach.id) : Promise.resolve(null as unknown as CoachDetailsDto),
    enabled: !!coach && modalType === "Edit" && open,
  });

  // Use the appropriate schema based on modalType
  const formSchema = modalType === "Add" ? createCoachCommandSchema : updateCoachCommandSchema;

  const {
    register,
    handleSubmit,
    formState: {errors},
    reset,
    setValue,
    watch,
  } = useForm({
    resolver: zodResolver(formSchema),
    mode: "onChange",
    defaultValues: modalType === "Add"
      ? {
        operatorId: operatorId,
        code: "",
        seatsNumbers: [],
      }
      : {
        id: coach?.id || "",
        operatorId: operatorId,
        name: "",
        seatsNumbers: [],
      },
  });

  // Update form when coach details are loaded
  useEffect(() => {
    if (modalType === "Edit" && coachDetails) {
      reset({
        id: coachDetails.id,
        operatorId: operatorId,
        name: coachDetails.code, // Using code as name for update
        seatsNumbers: coachDetails.seatsNumbers,
      });
      setSeatsNumbers(coachDetails.seatsNumbers);
    } else if (modalType === "Add") {
      reset({
        operatorId: operatorId,
        code: "",
        seatsNumbers: [],
      });
      setSeatsNumbers([]);
    }
  }, [coachDetails, modalType, operatorId, reset]);

  // Watch seatsNumbers to update the form value
  useEffect(() => {
    setValue('seatsNumbers', seatsNumbers);
  }, [seatsNumbers, setValue]);

  const handleAddSeat = () => {
    const seatNumberValue = parseInt(seatNumber);
    if (!isNaN(seatNumberValue) && !seatsNumbers.includes(seatNumberValue)) {
      setSeatsNumbers([...seatsNumbers, seatNumberValue].sort((a, b) => a - b));
      setSeatNumber("");
    }
  };

  const handleRemoveSeat = (seatToRemove: number) => {
    setSeatsNumbers(seatsNumbers.filter(seat => seat !== seatToRemove));
  };

  if (modalType === "Edit" && isLoading) return <Loader />;
  if (modalType === "Edit" && isError) return <ErrorDisplay />;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{modalType} Coach</DialogTitle>
          <DialogDescription>
            {modalType === "Add" ? "Create a new coach" : "Edit coach details"}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(handleSave)} className="space-y-6">
          {modalType === "Add" ? (
            <div className="flex flex-col">
              <Input id="code" placeholder="Code" {...register("code")} />
              {errors.code && <p className="text-red-500 text-sm">{errors.code.message}</p>}
            </div>
          ) : (
            <div className="flex flex-col">
              <Input id="name" placeholder="Name" {...register("name")} />
              {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
            </div>
          )}

          <div className="flex flex-col">
            <label htmlFor="seats" className="text-sm font-medium mb-1">Seats</label>
            <div className="flex gap-2">
              <Input
                id="seats"
                type="number"
                placeholder="Seat number"
                value={seatNumber}
                onChange={(e) => setSeatNumber(e.target.value)}
              />
              <Button type="button" onClick={handleAddSeat}>Add</Button>
            </div>

            <div className="flex flex-wrap gap-2 mt-2">
              {seatsNumbers.map((seat) => (
                <div key={seat} className="relative flex items-center justify-center w-10 h-10 border border-gray-300 rounded-md">
                  {seat}
                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    className="absolute -top-2 -right-2 h-4 w-4 bg-white rounded-full"
                    onClick={() => handleRemoveSeat(seat)}
                  >
                    <X className="h-3 w-3" />
                  </Button>
                </div>
              ))}
            </div>
            {errors.seatsNumbers && <p className="text-red-500 text-sm">{errors.seatsNumbers.message}</p>}
          </div>

          <div className="flex flex-col">
            <Button type="submit">Save</Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
