import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {DeleteIcon, Settings2Icon} from "lucide-react";
import {Discount} from "@/components/types/discount.ts";

type DiscountListProps = {
  discounts: Discount[];
  onEdit: (discount: Discount) => void;
  onDelete: (discount: Discount) => void;
}

export function DiscountsList(props: DiscountListProps) {
  const {discounts, onEdit, onDelete} = props;

  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead className="w-2/5">Name</TableHead>
          <TableCell className="w-2/5">Percentage</TableCell>
          <TableCell className="w-1/5">Actions</TableCell>
        </TableRow>
      </TableHeader>
      <TableBody>
        {discounts.map(discount => (
          <TableRow key={discount.id}>
            <TableCell className="w-2/5">{discount.name}</TableCell>
            <TableCell className="w-2/5">{discount.percentage}</TableCell>
            <TableCell className="w-1/5">
              <div className="flex gap-x-2">
                <Button size="icon" onClick={() => onEdit(discount)}><Settings2Icon/></Button>
                <Button size="icon" onClick={() => onDelete(discount)}><DeleteIcon/></Button>
              </div>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}