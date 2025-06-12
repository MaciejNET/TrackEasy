import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {CheckIcon, XIcon, InfoIcon} from "lucide-react";
import {ConnectionChangeRequestDto, ConnectionRequestType} from "@/schemas/connection-change-request-schema.ts";
import {useConnectionChangeRequestStore} from "@/stores/connection-change-request-store.ts";
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {fetchConnectionChangeRequests} from "@/api/connection-change-requests-api.ts";
import {Loader} from "@/components/loader.tsx";
import {Paginator} from "@/components/paginator.tsx";
import {NoData} from "@/components/no-data.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";
import {Badge} from "@/components/ui/badge.tsx";

type ConnectionChangeRequestsListProps = {
  onDetails: (request: ConnectionChangeRequestDto) => void;
  onApprove: (request: ConnectionChangeRequestDto) => void;
  onReject: (request: ConnectionChangeRequestDto) => void;
}

export function ConnectionChangeRequestsList(props: ConnectionChangeRequestsListProps) {
  const {onDetails, onApprove, onReject} = props;
  const {searchParams, setSearchParams} = useConnectionChangeRequestStore();

  const {data, isLoading, isError} = useQuery({
    queryKey: ['connection-change-requests', searchParams],
    queryFn: () => fetchConnectionChangeRequests(searchParams),
    placeholderData: keepPreviousData,
  });

  const currentPage = searchParams.pageNumber;
  const totalPages = data?.totalPages ?? 1;

  const handlePageChange = (page: number) => {
    setSearchParams({pageNumber: page});
  }

  const getRequestTypeBadge = (requestType: ConnectionRequestType) => {
    switch (requestType) {
      case ConnectionRequestType.ADD:
        return <Badge className="bg-green-500">Add</Badge>;
      case ConnectionRequestType.UPDATE:
        return <Badge className="bg-blue-500">Update</Badge>;
      case ConnectionRequestType.DELETE:
        return <Badge className="bg-red-500">Delete</Badge>;
      default:
        return null;
    }
  };

  if (isLoading) return <Loader/>;
  if (isError) return <ErrorDisplay/>
  if (data?.items.length === 0) return <NoData/>

  return (
    <>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead className="w-1/5">Name</TableHead>
            <TableHead className="w-1/5">Operator</TableHead>
            <TableHead className="w-1/5">Start Station</TableHead>
            <TableHead className="w-1/5">End Station</TableHead>
            <TableHead className="w-1/10">Type</TableHead>
            <TableHead className="w-1/5">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.items.map(request => (
            <TableRow key={request.connectionId}>
              <TableCell className="w-1/5">{request.name}</TableCell>
              <TableCell className="w-1/5">{request.operatorName}</TableCell>
              <TableCell className="w-1/5">{request.startStation}</TableCell>
              <TableCell className="w-1/5">{request.endStation}</TableCell>
              <TableCell className="w-1/10">{getRequestTypeBadge(request.requestType)}</TableCell>
              <TableCell className="w-1/5">
                <div className="flex gap-x-2">
                  <Button size="icon" onClick={() => onDetails(request)}><InfoIcon/></Button>
                  <Button size="icon" variant="success" onClick={() => onApprove(request)}><CheckIcon/></Button>
                  <Button size="icon" variant="destructive" onClick={() => onReject(request)}><XIcon/></Button>
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