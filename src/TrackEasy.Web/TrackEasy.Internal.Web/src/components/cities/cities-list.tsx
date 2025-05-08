import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import {Button} from "@/components/ui/button.tsx";
import {DeleteIcon, Settings2Icon} from "lucide-react";
import {City} from "@/schemas/city-schema.ts";
import {useCityStore} from "@/stores/city-store.ts";
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {fetchCities} from "@/api/cities-api.ts";
import {Loader} from "@/components/loader.tsx";
import {Paginator} from "@/components/paginator.tsx";
import {NoData} from "@/components/no-data.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";

type CitiesListProps = {
  onEdit: (city: City) => void;
  onDelete: (city: City) => void;
}

export function CitiesList(props: CitiesListProps) {
  const {onEdit, onDelete} = props;
  const {searchParams, setSearchParams} = useCityStore();

  const {data, isLoading, isError} = useQuery({
    queryKey: ['cities', searchParams],
    queryFn: () => fetchCities(searchParams),
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
            <TableCell className="w-1/5">Actions</TableCell>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.items.map(city => (
            <TableRow key={city.id}>
              <TableCell className="w-4/5">{city.name}</TableCell>
              <TableCell className="w-1/5">
                <div className="flex gap-x-2">
                  <Button size="icon" onClick={() => onEdit(city)}><Settings2Icon/></Button>
                  <Button size="icon" onClick={() => onDelete(city)}><DeleteIcon/></Button>
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