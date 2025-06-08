import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { SystemListItemDto } from "@/api/system-lists-api";
import { Button } from "@/components/ui/button";
import { Pencil, Trash2 } from "lucide-react";

interface ManagersListProps {
  managers: SystemListItemDto[];
  onEdit?: (manager: SystemListItemDto) => void;
  onDelete?: (manager: SystemListItemDto) => void;
}

export function ManagersList({ managers, onEdit, onDelete }: ManagersListProps) {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead className="w-3/4">Name</TableHead>
          <TableHead className="w-1/4">Actions</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {managers.map((manager) => (
          <TableRow key={manager.id}>
            <TableCell>{manager.name}</TableCell>
            <TableCell>
              <div className="flex gap-x-2">
                {onEdit && (
                  <Button size="icon" variant="outline" onClick={() => onEdit(manager)}>
                    <Pencil className="h-4 w-4" />
                  </Button>
                )}
                {onDelete && (
                  <Button size="icon" variant="outline" onClick={() => onDelete(manager)}>
                    <Trash2 className="h-4 w-4" />
                  </Button>
                )}
              </div>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}
