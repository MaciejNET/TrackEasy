import {Dialog, DialogContent, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {Button} from "@/components/ui/button.tsx";

type DeleteCityProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  onDelete: () => void;
  onCancel: () => void;
}

export function DeleteCity(props: DeleteCityProps) {
  const {open, setOpen, onDelete, onCancel} = props;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Delete City</DialogTitle>
        </DialogHeader>
        <div className="flex flex-col">
          <p>Are you sure you want to delete this city?</p>
          <div className="flex gap-x-2 mt-2">
            <Button type="button" variant="outline" onClick={onCancel}>Cancel</Button>
            <Button type="button" onClick={onDelete}>Delete</Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  )
}