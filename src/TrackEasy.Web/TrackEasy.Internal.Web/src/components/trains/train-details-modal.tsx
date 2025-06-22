import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {TrainDto, TrainDetailsDto} from "@/schemas/train-schema.ts";
import {useQuery} from "@tanstack/react-query";
import {fetchTrain} from "@/api/trains-api.ts";
import {Loader} from "@/components/loader.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";

type TrainDetailsModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  train: TrainDto | null;
  operatorId: string;
};

export function TrainDetailsModal(props: TrainDetailsModalProps) {
  const {open, setOpen, train, operatorId} = props;

  const {
    data: trainDetails,
    isLoading,
    isError
  } = useQuery({
    queryKey: ['train-details', operatorId, train?.id],
    queryFn: () => train ? fetchTrain(operatorId, train.id) : Promise.resolve(null as unknown as TrainDetailsDto),
    enabled: !!train && open,
  });

  if (!train) return null;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Train Details</DialogTitle>
          <DialogDescription>
            View detailed information about this train
          </DialogDescription>
        </DialogHeader>
        {isLoading ? (
          <Loader />
        ) : isError ? (
          <ErrorDisplay />
        ) : trainDetails ? (
          <div className="space-y-4">
            <div>
              <h3 className="text-sm font-medium">Name</h3>
              <p className="text-lg">{trainDetails.name}</p>
            </div>
            <div>
              <h3 className="text-sm font-medium">Coaches</h3>
              {trainDetails.coaches.length > 0 ? (
                <div className="mt-2">
                  <table className="w-full border-collapse">
                    <thead>
                      <tr className="border-b">
                        <th className="text-left py-2">Coach Code</th>
                        <th className="text-left py-2">Number</th>
                      </tr>
                    </thead>
                    <tbody>
                      {trainDetails.coaches.map((coachData) => {
                        
                        const [coach, number] = Array.isArray(coachData) 
                          ? coachData 
                          : [coachData.coach, coachData.number];

                        return (
                          <tr key={coach.id} className="border-b">
                            <td className="py-2">{coach.code}</td>
                            <td className="py-2">{number}</td>
                          </tr>
                        );
                      })}
                    </tbody>
                  </table>
                </div>
              ) : (
                <p className="text-sm text-gray-500 mt-1">No coaches assigned to this train</p>
              )}
            </div>
          </div>
        ) : null}
      </DialogContent>
    </Dialog>
  );
}
