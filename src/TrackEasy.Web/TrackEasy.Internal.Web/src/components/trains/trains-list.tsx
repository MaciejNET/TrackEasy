import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {DeleteIcon, Settings2Icon, InfoIcon} from "lucide-react";
import {TrainDto} from "@/schemas/train-schema.ts";
import {useTrainStore} from "@/stores/train-store.ts";
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {fetchTrains} from "@/api/trains-api.ts";
import {Loader} from "@/components/loader.tsx";
import {Paginator} from "@/components/paginator.tsx";
import {NoData} from "@/components/no-data.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";

type TrainsListProps = {
  operatorId: string;
  onEdit: (train: TrainDto) => void;
  onDelete: (train: TrainDto) => void;
  onDetails?: (train: TrainDto) => void;
}

export function TrainsList(props: TrainsListProps) {
  const {operatorId, onEdit, onDelete, onDetails} = props;
  const {searchParams, setSearchParams} = useTrainStore();

  const {data, isLoading, isError} = useQuery({
    queryKey: ['trains', operatorId, searchParams],
    queryFn: () => fetchTrains(operatorId, searchParams),
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
            <TableHead className="w-4/5">Name</TableHead>
            <TableHead className="w-1/5">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.items.map(train => (
            <TableRow key={train.id}>
              <TableCell className="w-4/5">{train.name}</TableCell>
              <TableCell className="w-1/5">
                <div className="flex gap-x-2">
                  {onDetails && (
                    <Button size="icon" onClick={() => onDetails(train)}><InfoIcon/></Button>
                  )}
                  <Button size="icon" onClick={() => onEdit(train)}><Settings2Icon/></Button>
                  <Button size="icon" onClick={() => onDelete(train)}><DeleteIcon/></Button>
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