import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {CheckIcon, XIcon, InfoIcon} from "lucide-react";
import {RefundRequestDto} from "@/schemas/refund-request-schema.ts";
import {useRefundRequestStore} from "@/stores/refund-request-store.ts";
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {fetchRefundRequests} from "@/api/refund-requests-api.ts";
import {Loader} from "@/components/loader.tsx";
import {Paginator} from "@/components/paginator.tsx";
import {NoData} from "@/components/no-data.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";
import {format} from "date-fns";

type RefundRequestsListProps = {
  onDetails: (request: RefundRequestDto) => void;
  onApprove: (request: RefundRequestDto) => void;
  onReject: (request: RefundRequestDto) => void;
}

export function RefundRequestsList(props: RefundRequestsListProps) {
  const {onDetails, onApprove, onReject} = props;
  const {searchParams, setSearchParams} = useRefundRequestStore();

  const {data, isLoading, isError} = useQuery({
    queryKey: ['refund-requests', searchParams],
    queryFn: () => fetchRefundRequests(searchParams.operatorId, searchParams),
    placeholderData: keepPreviousData,
    enabled: !!searchParams.operatorId,
  });

  const currentPage = searchParams.pageNumber;
  const totalPages = data?.totalPages ?? 1;

  const handlePageChange = (page: number) => {
    setSearchParams({pageNumber: page});
  }

  const formatDate = (dateString: string) => {
    try {
      return dateString;
    } catch (error) {
      return dateString;
    }
  };

  const formatDateTime = (dateTimeString: string) => {
    try {
      return dateTimeString;
    } catch (error) {
      return dateTimeString;
    }
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
            <TableHead className="w-1/6">Ticket #</TableHead>
            <TableHead className="w-1/6">Email</TableHead>
            <TableHead className="w-1/6">Connection Date</TableHead>
            <TableHead className="w-1/4">Reason</TableHead>
            <TableHead className="w-1/6">Created At</TableHead>
            <TableHead className="w-1/6">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.items.map(request => (
            <TableRow key={request.id}>
              <TableCell className="w-1/6">{request.ticketNumber}</TableCell>
              <TableCell className="w-1/6">{request.emailAddress}</TableCell>
              <TableCell className="w-1/6">{formatDate(request.connectionDate)}</TableCell>
              <TableCell className="w-1/4">{request.reason}</TableCell>
              <TableCell className="w-1/6">{formatDateTime(request.createdAt)}</TableCell>
              <TableCell className="w-1/6">
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