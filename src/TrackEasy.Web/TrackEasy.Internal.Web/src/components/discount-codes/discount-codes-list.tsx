import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {DeleteIcon, Settings2Icon} from "lucide-react";
import {DiscountCodeDto} from "@/schemas/discount-code-schema.ts";
import {useDiscountCodeStore} from "@/stores/discount-code-store.ts";
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {fetchDiscountCodes} from "@/api/discount-codes-api.ts";
import {Loader} from "@/components/loader.tsx";
import {Paginator} from "@/components/paginator.tsx";
import {NoData} from "@/components/no-data.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";
import {format} from "date-fns";

type DiscountCodesListProps = {
  onEdit: (discountCode: DiscountCodeDto) => void;
  onDelete: (discountCode: DiscountCodeDto) => void;
}

export function DiscountCodesList(props: DiscountCodesListProps) {
  const {onEdit, onDelete} = props;
  const {searchParams, setSearchParams} = useDiscountCodeStore();

  const {data, isLoading, isError} = useQuery({
    queryKey: ['discount-codes', searchParams],
    queryFn: () => fetchDiscountCodes(searchParams),
    placeholderData: keepPreviousData,
  });

  const currentPage = searchParams.pageNumber;
  const totalPages = data?.totalPages ?? 1;

  const handlePageChange = (page: number) => {
    setSearchParams({pageNumber: page});
  }

  if (isLoading) return <Loader/>;
  if (isError) return <ErrorDisplay/>
  if (data?.items.length === 0) return <NoData/>

  return (
    <>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead className="w-1/5">Code</TableHead>
            <TableHead className="w-1/5">Percentage</TableHead>
            <TableHead className="w-1/5">Valid From</TableHead>
            <TableHead className="w-1/5">Valid To</TableHead>
            <TableHead className="w-1/5">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.items.map(discountCode => (
            <TableRow key={discountCode.id}>
              <TableCell className="w-1/5">{discountCode.code}</TableCell>
              <TableCell className="w-1/5">{discountCode.percentage}%</TableCell>
              <TableCell className="w-1/5">{format(new Date(discountCode.from), 'PPP')}</TableCell>
              <TableCell className="w-1/5">{format(new Date(discountCode.to), 'PPP')}</TableCell>
              <TableCell className="w-1/5">
                <div className="flex gap-x-2">
                  <Button size="icon" onClick={() => onEdit(discountCode)}><Settings2Icon/></Button>
                  <Button size="icon" onClick={() => onDelete(discountCode)}><DeleteIcon/></Button>
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