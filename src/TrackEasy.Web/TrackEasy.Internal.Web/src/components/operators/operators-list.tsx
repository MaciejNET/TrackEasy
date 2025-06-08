import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {DeleteIcon, Settings2Icon, InfoIcon} from "lucide-react";
import {OperatorDto} from "@/schemas/operator-schema.ts";
import {useOperatorStore} from "@/stores/operator-store.ts";
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {fetchOperators} from "@/api/operators-api.ts";
import {Loader} from "@/components/loader.tsx";
import {Paginator} from "@/components/paginator.tsx";
import {NoData} from "@/components/no-data.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";

type OperatorsListProps = {
  onEdit: (operator: OperatorDto) => void;
  onDelete: (operator: OperatorDto) => void;
  onDetails?: (operator: OperatorDto) => void;
}

export function OperatorsList(props: OperatorsListProps) {
  const {onEdit, onDelete, onDetails} = props;
  const {searchParams, setSearchParams} = useOperatorStore();

  const {data, isLoading, isError} = useQuery({
    queryKey: ['operators', searchParams],
    queryFn: () => fetchOperators(searchParams),
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
            <TableHead className="w-2/5">Name</TableHead>
            <TableHead className="w-2/5">Code</TableHead>
            <TableHead className="w-1/5">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.items.map(operator => (
            <TableRow key={operator.id}>
              <TableCell className="w-2/5">{operator.name}</TableCell>
              <TableCell className="w-2/5">{operator.code}</TableCell>
              <TableCell className="w-1/5">
                <div className="flex gap-x-2">
                  {onDetails && (
                    <Button size="icon" onClick={() => onDetails(operator)}><InfoIcon/></Button>
                  )}
                  <Button size="icon" onClick={() => onEdit(operator)}><Settings2Icon/></Button>
                  <Button size="icon" onClick={() => onDelete(operator)}><DeleteIcon/></Button>
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