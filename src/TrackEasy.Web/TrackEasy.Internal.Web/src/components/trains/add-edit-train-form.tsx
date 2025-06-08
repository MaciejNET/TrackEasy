import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {ModalType} from "@/types/modals.ts";
import {useEffect, useState} from "react";
import {
  TrainDto,
  TrainDetailsDto,
  AddTrainCommand,
  addTrainCommandSchema,
  UpdateTrainCommand,
  updateTrainCommandSchema,
  CoachDto,
  CoachSelection
} from "@/schemas/train-schema.ts";
import {useQuery} from "@tanstack/react-query";
import {fetchTrain} from "@/api/trains-api.ts";
import {Loader} from "@/components/loader.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";
import {X, Plus} from "lucide-react";
import {CoachSelector} from "@/components/trains/coach-selector.tsx";

type AddEditTrainFormProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  handleSave: (train: AddTrainCommand | UpdateTrainCommand) => void;
  modalType: ModalType | null;
  train: TrainDto | null;
  operatorId: string;
};

export function AddEditTrainForm(props: AddEditTrainFormProps) {
  const {open, setOpen, handleSave, modalType, train, operatorId} = props;
  const [selectedCoaches, setSelectedCoaches] = useState<Map<string, {coach: CoachDto, number: number}>>(new Map());
  const [coachNumber, setCoachNumber] = useState<string>("");

  // Fetch train details if editing
  const {
    data: trainDetails,
    isLoading,
    isError
  } = useQuery({
    queryKey: ['train-details-for-edit', operatorId, train?.id],
    queryFn: () => train ? fetchTrain(operatorId, train.id) : Promise.resolve(null as unknown as TrainDetailsDto),
    enabled: !!train && modalType === "Edit" && open,
  });

  // Use the appropriate schema based on modalType
  const formSchema = modalType === "Add" ? addTrainCommandSchema : updateTrainCommandSchema;

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
        name: "",
        coaches: [],
      }
      : {
        operatorId: operatorId,
        trainId: train?.id || "",
        name: train?.name || "",
        coaches: [],
      },
  });

  // Update form when train details are loaded
  useEffect(() => {
    if (modalType === "Edit" && trainDetails) {
      // Convert coaches array to Map for easier manipulation
      const coachesMap = new Map();
      trainDetails.coaches.forEach((coachData) => {
        // Handle both tuple [coach, number] and object {coach, number} formats
        const [coach, number] = Array.isArray(coachData) 
          ? coachData 
          : [coachData.coach, coachData.number];

        coachesMap.set(coach.id, {coach, number});
      });
      setSelectedCoaches(coachesMap);

      reset({
        operatorId: operatorId,
        trainId: trainDetails.id,
        name: trainDetails.name,
        coaches: Array.from(coachesMap.entries()).map(([id, {coach, number}]) => [id, number] as [string, number]),
      });
    } else if (modalType === "Add") {
      setSelectedCoaches(new Map());
      reset({
        operatorId: operatorId,
        name: "",
        coaches: [],
      });
    }
  }, [trainDetails, modalType, operatorId, reset]);

  // Update coaches array in form when selectedCoaches changes
  useEffect(() => {
    const coachesArray = Array.from(selectedCoaches.entries()).map(([id, {number}]) => [id, number] as [string, number]);
    setValue(modalType === "Add" ? 'coaches' : 'coaches', coachesArray);
  }, [selectedCoaches, setValue, modalType]);

  const handleAddCoach = (coach: CoachDto) => {
    if (coachNumber && !isNaN(parseInt(coachNumber))) {
      const number = parseInt(coachNumber);
      setSelectedCoaches(new Map(selectedCoaches.set(coach.id, {coach, number})));
      setCoachNumber("");
    }
  };

  const handleRemoveCoach = (coachId: string) => {
    const newSelectedCoaches = new Map(selectedCoaches);
    newSelectedCoaches.delete(coachId);
    setSelectedCoaches(newSelectedCoaches);
  };

  if (modalType === "Edit" && isLoading) return <Loader />;
  if (modalType === "Edit" && isError) return <ErrorDisplay />;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className="max-w-3xl">
        <DialogHeader>
          <DialogTitle>{modalType} Train</DialogTitle>
          <DialogDescription>
            {modalType === "Add" ? "Create a new train" : "Edit train details"}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(handleSave)} className="space-y-6">
          <div className="flex flex-col">
            <label htmlFor="name" className="text-sm font-medium mb-1">Name</label>
            <Input id="name" placeholder="Train Name" {...register("name")} />
            {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Coaches</label>
            <div className="flex gap-2 mb-4">
              <div className="flex-1">
                <CoachSelector 
                  operatorId={operatorId} 
                  onSelect={handleAddCoach} 
                />
              </div>
              <div className="w-24">
                <Input
                  type="number"
                  placeholder="Number"
                  value={coachNumber}
                  onChange={(e) => setCoachNumber(e.target.value)}
                  min="1"
                />
              </div>
            </div>

            {selectedCoaches.size > 0 ? (
              <div className="border rounded-md p-4">
                <h4 className="text-sm font-medium mb-2">Selected Coaches</h4>
                <div className="space-y-2">
                  {Array.from(selectedCoaches.entries()).map(([id, {coach, number}]) => (
                    <div key={id} className="flex items-center justify-between p-2 border border-gray-300 rounded-md">
                      <div className="flex items-center gap-2">
                        <span className="font-medium">{coach.code}</span>
                        <span className="text-gray-500">Number: {number}</span>
                      </div>
                      <Button
                        type="button"
                        variant="ghost"
                        size="icon"
                        onClick={() => handleRemoveCoach(id)}
                      >
                        <X className="h-4 w-4" />
                      </Button>
                    </div>
                  ))}
                </div>
              </div>
            ) : (
              <div className="text-center p-4 border border-dashed rounded-md">
                <p className="text-gray-500">No coaches selected. Add coaches to this train.</p>
              </div>
            )}

            {errors.coaches && <p className="text-red-500 text-sm">{errors.coaches.message}</p>}
          </div>

          <div className="flex justify-end gap-2">
            <Button type="button" variant="outline" onClick={() => setOpen(false)}>Cancel</Button>
            <Button type="submit">Save</Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
