import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {DeleteIcon, Settings2Icon, InfoIcon} from "lucide-react";
import {CoachDto} from "@/schemas/coach-schema.ts";
import {useCoachStore} from "@/stores/coach-store.ts";
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {fetchCoaches} from "@/api/coaches-api.ts";
import {Loader} from "@/components/loader.tsx";
import {Paginator} from "@/components/paginator.tsx";
import {NoData} from "@/components/no-data.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";

type CoachesListProps = {
  operatorId: string;
  onEdit: (coach: CoachDto) => void;
  onDelete: (coach: CoachDto) => void;
  onDetails?: (coach: CoachDto) => void;
}

export function CoachesList(props: CoachesListProps) {
  const {operatorId, onEdit, onDelete, onDetails} = props;
  const {searchParams, setSearchParams} = useCoachStore();

  const {data, isLoading, isError} = useQuery({
    queryKey: ['coaches', operatorId, searchParams],
    queryFn: () => fetchCoaches(operatorId, searchParams),
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
            <TableHead className="w-4/5">Code</TableHead>
            <TableHead className="w-1/5">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.items.map(coach => (
            <TableRow key={coach.id}>
              <TableCell className="w-4/5">{coach.code}</TableCell>
              <TableCell className="w-1/5">
                <div className="flex gap-x-2">
                  {onDetails && (
                    <Button size="icon" onClick={() => onDetails(coach)}><InfoIcon/></Button>
                  )}
                  <Button size="icon" onClick={() => onEdit(coach)}><Settings2Icon/></Button>
                  <Button size="icon" onClick={() => onDelete(coach)}><DeleteIcon/></Button>
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