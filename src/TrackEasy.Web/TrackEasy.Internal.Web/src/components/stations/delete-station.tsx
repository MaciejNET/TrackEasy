import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {Button} from "@/components/ui/button.tsx";

type DeleteStationProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  onDelete: () => void;
  onCancel: () => void;
}

export function DeleteStation(props: DeleteStationProps) {
  const {open, setOpen, onDelete, onCancel} = props;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Delete Station</DialogTitle>
          <DialogDescription>
            This action cannot be undone
          </DialogDescription>
        </DialogHeader>
        <div className="flex flex-col">
          <p>Are you sure you want to delete this station?</p>
          <div className="flex gap-x-2 mt-2">
            <Button type="button" variant="outline" onClick={onCancel}>Cancel</Button>
            <Button type="button" onClick={onDelete}>Delete</Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  )
}
