import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {DeleteIcon, Settings2Icon, InfoIcon} from "lucide-react";
import {StationDto} from "@/schemas/station-schema.ts";
import {useStationStore} from "@/stores/station-store.ts";
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {fetchStations} from "@/api/stations-api.ts";
import {Loader} from "@/components/loader.tsx";
import {Paginator} from "@/components/paginator.tsx";
import {NoData} from "@/components/no-data.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";

type StationsListProps = {
  onEdit: (station: StationDto) => void;
  onDelete: (station: StationDto) => void;
  onDetails: (station: StationDto) => void;
}

export function StationsList(props: StationsListProps) {
  const {onEdit, onDelete, onDetails} = props;
  const {searchParams, setSearchParams} = useStationStore();

  const {data, isLoading, isError} = useQuery({
    queryKey: ['stations', searchParams],
    queryFn: () => fetchStations(searchParams),
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
            <TableHead className="w-2/5">City</TableHead>
            <TableHead className="w-1/5">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.items.map(station => (
            <TableRow key={station.id}>
              <TableCell className="w-2/5">{station.name}</TableCell>
              <TableCell className="w-2/5">{station.city}</TableCell>
              <TableCell className="w-1/5">
                <div className="flex gap-x-2">
                  <Button size="icon" onClick={() => onDetails(station)}><InfoIcon/></Button>
                  <Button size="icon" onClick={() => onEdit(station)}><Settings2Icon/></Button>
                  <Button size="icon" onClick={() => onDelete(station)}><DeleteIcon/></Button>
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