import {Dialog, DialogContent, DialogHeader, DialogTitle} from "@/components/ui/dialog.tsx";
import {Button} from "@/components/ui/button.tsx";

type DeleteDiscountProps = {
  open: boolean;
  setOpen: (open: boolean) => void;
  discountId: string;
  onDelete: (id: string) => void;
  onCancel: () => void;
}

export function DeleteDiscount(props: DeleteDiscountProps) {
  const {open, setOpen, discountId, onDelete, onCancel} = props;

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Delete Discount</DialogTitle>
        </DialogHeader>
        <div className="flex flex-col">
          <p>Are you sure you want to delete this discount?</p>
          <div className="flex gap-x-2 mt-2">
            <Button type="button" variant="outline" onClick={onCancel}>Cancel</Button>
            <Button type="button" onClick={() => onDelete(discountId)}>Delete</Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  )
}