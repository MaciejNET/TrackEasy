import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {InfoIcon, PencilIcon, CalendarIcon, CheckCircle, XCircle} from "lucide-react";
import {ConnectionDto} from "@/schemas/connection-schema.ts";
import {useConnectionStore} from "@/stores/connection-store.ts";
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {fetchConnections} from "@/api/connections-api.ts";
import {Loader} from "@/components/loader.tsx";
import {Paginator} from "@/components/paginator.tsx";
import {NoData} from "@/components/no-data.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";
import {Badge} from "@/components/ui/badge.tsx";

type ConnectionsListProps = {
  onDetails: (connection: ConnectionDto) => void;
  onEdit: (connection: ConnectionDto) => void;
  onEditSchedule: (connection: ConnectionDto) => void;
}

export function ConnectionsList(props: ConnectionsListProps) {
  const {onDetails, onEdit, onEditSchedule} = props;
  const {searchParams, setSearchParams} = useConnectionStore();

  const {data, isLoading, isError} = useQuery({
    queryKey: ['connections', searchParams],
    queryFn: () => fetchConnections(searchParams.operatorId, searchParams),
    placeholderData: keepPreviousData,
    enabled: !!searchParams.operatorId,
  });

  const currentPage = searchParams.pageNumber;
  const totalPages = data?.totalPages ?? 1;

  const handlePageChange = (page: number) => {
    setSearchParams({pageNumber: page});
  }

  const formatDaysOfWeek = (days: string[]) => {
    const dayMap: Record<string, string> = {
      "0": "sun",
      "1": "mon",
      "2": "tue",
      "3": "wed",
      "4": "thu",
      "5": "fri",
      "6": "sat",
      "Sunday": "sun",
      "Monday": "mon",
      "Tuesday": "tue",
      "Wednesday": "wed",
      "Thursday": "thu",
      "Friday": "fri",
      "Saturday": "sat"
    };

    return days.map(day => {
      if (typeof day === 'string') {
        return dayMap[day] || day.substring(0, 3).toLowerCase();
      }
      // Handle non-string values by converting them to string
      return dayMap[String(day)] || String(day);
    }).join(', ');
  };

  if (!searchParams.operatorId) return <div className="text-center p-6">Please select an operator</div>;
  if (isLoading) return <Loader/>;
  if (isError) return <ErrorDisplay/>
  if (data?.items.length === 0) return <NoData/>

  return (
    <>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead className="w-1/5">Name</TableHead>
            <TableHead className="w-1/5">Start Station</TableHead>
            <TableHead className="w-1/5">End Station</TableHead>
            <TableHead className="w-1/5">Days of Week</TableHead>
            <TableHead className="w-1/10">Status</TableHead>
            <TableHead className="w-1/5">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.items.map(connection => (
            <TableRow key={connection.id}>
              <TableCell className="w-1/5">{connection.name}</TableCell>
              <TableCell className="w-1/5">{connection.startStation}</TableCell>
              <TableCell className="w-1/5">{connection.endStation}</TableCell>
              <TableCell className="w-1/5">{formatDaysOfWeek(connection.daysOfWeek)}</TableCell>
              <TableCell className="w-1/10">
                {connection.isActive ? 
                  <Badge className="bg-green-500"><CheckCircle className="h-3 w-3 mr-1" /> Active</Badge> : 
                  <Badge className="bg-red-500"><XCircle className="h-3 w-3 mr-1" /> Inactive</Badge>}
              </TableCell>
              <TableCell className="w-1/5">
                <div className="flex gap-x-2">
                  <Button size="icon" onClick={() => onDetails(connection)}><InfoIcon/></Button>
                  <Button size="icon" onClick={() => onEdit(connection)}><PencilIcon/></Button>
                  <Button size="icon" onClick={() => onEditSchedule(connection)}><CalendarIcon/></Button>
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
