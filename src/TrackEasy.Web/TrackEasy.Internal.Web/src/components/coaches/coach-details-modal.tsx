import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {CoachDto, CoachDetailsDto} from "@/schemas/coach-schema.ts";
import {useQuery} from "@tanstack/react-query";
import {fetchCoach} from "@/api/coaches-api.ts";
import {Loader} from "@/components/loader.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";

type CoachDetailsModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  coach: CoachDto | null;
  operatorId: string;
};

export function CoachDetailsModal(props: CoachDetailsModalProps) {
  const {open, setOpen, coach, operatorId} = props;

  const {
    data: coachDetails,
    isLoading,
    isError
  } = useQuery({
    queryKey: ['coach-details', operatorId, coach?.id],
    queryFn: () => coach ? fetchCoach(operatorId, coach.id) : Promise.resolve(null as unknown as CoachDetailsDto),
    enabled: !!coach && open,
  });

  if (!coach) return null;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Coach Details</DialogTitle>
          <DialogDescription>
            View detailed information about this coach
          </DialogDescription>
        </DialogHeader>
        {isLoading ? (
          <Loader />
        ) : isError ? (
          <ErrorDisplay />
        ) : coachDetails ? (
          <div className="space-y-4">
            <div>
              <h3 className="text-sm font-medium">Code</h3>
              <p className="text-lg">{coachDetails.code}</p>
            </div>
            <div>
              <h3 className="text-sm font-medium">Seats</h3>
              <div className="flex flex-wrap gap-2 mt-2">
                {coachDetails.seatsNumbers.map((seatNumber) => (
                  <div key={seatNumber} className="flex items-center justify-center w-10 h-10 border border-gray-300 rounded-md">
                    {seatNumber}
                  </div>
                ))}
              </div>
            </div>
          </div>
        ) : null}
      </DialogContent>
    </Dialog>
  );
}
