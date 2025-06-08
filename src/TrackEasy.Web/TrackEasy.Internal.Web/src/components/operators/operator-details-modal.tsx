import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {OperatorDto} from "@/schemas/operator-schema.ts";

type OperatorDetailsModalProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  operator: OperatorDto | null;
};

export function OperatorDetailsModal(props: OperatorDetailsModalProps) {
  const {open, setOpen, operator} = props;

  if (!operator) return null;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Operator Details</DialogTitle>
          <DialogDescription>
            View detailed information about this operator
          </DialogDescription>
        </DialogHeader>
        <div className="space-y-4">
          <div>
            <h3 className="text-sm font-medium">Name</h3>
            <p className="text-lg">{operator.name}</p>
          </div>
          <div>
            <h3 className="text-sm font-medium">Code</h3>
            <p className="text-lg">{operator.code}</p>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}