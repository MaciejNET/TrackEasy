import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {DeleteIcon, Settings2Icon} from "lucide-react";
import {Discount} from "@/schemas/discount-schema.ts";
import {useDiscountStore} from "@/stores/discount-store.ts";
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {fetchDiscounts} from "@/api/discounts-api.ts";
import {Loader} from "@/components/loader.tsx";
import {Paginator} from "@/components/paginator.tsx";

type DiscountListProps = {
  onEdit: (discount: Discount) => void;
  onDelete: (discount: Discount) => void;
}

export function DiscountsList(props: DiscountListProps) {
  const {onEdit, onDelete} = props;
  const {searchParams, setSearchParams} = useDiscountStore();

  const {data, isLoading, isError} = useQuery({
    queryKey: ['discounts', searchParams],
    queryFn: () => fetchDiscounts(searchParams),
    placeholderData: keepPreviousData
  });

  const currentPage = searchParams.pageNumber;
  const totalPages = data?.totalPages ?? 1;

  const handlePageChange = (page: number) => {
    setSearchParams({pageNumber: page});
  }

  if (isLoading) return <Loader/>;
  if (isError) return <div>Error</div>

  return (
    <>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead className="w-2/5">Name</TableHead>
            <TableCell className="w-2/5">Percentage</TableCell>
            <TableCell className="w-1/5">Actions</TableCell>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.items.map(discount => (
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

      <Paginator
        currentPage={currentPage}
        totalPages={totalPages}
        onPageChange={handlePageChange}
      />
    </>
  );
}