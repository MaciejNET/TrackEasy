import {useState, useEffect} from "react";
import {useQuery} from "@tanstack/react-query";
import {fetchTrains} from "@/api/trains-api.ts";
import {TrainDto} from "@/schemas/train-schema.ts";
import {Button} from "@/components/ui/button.tsx";
import {Loader} from "@/components/loader.tsx";
import {ErrorDisplay} from "@/components/error-display.tsx";
import {Check, ChevronsUpDown} from "lucide-react";
import {cn} from "@/lib/utils.ts";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "@/components/ui/command";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";

type TrainSelectorProps = {
  operatorId: string;
  onSelect: (train: TrainDto) => void;
  disabled?: boolean;
};

export function TrainSelector(props: TrainSelectorProps) {
  const {operatorId, onSelect, disabled} = props;
  const [open, setOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [page, setPage] = useState(1);
  const [accumulatedTrains, setAccumulatedTrains] = useState<TrainDto[]>([]);
  const pageSize = 10;

  const {
    data,
    isLoading,
    isError,
    isFetching
  } = useQuery({
    queryKey: ['trains-for-selector', operatorId, searchTerm, page],
    queryFn: () => fetchTrains(operatorId, {
      trainName: searchTerm,
      pageNumber: page,
      pageSize: pageSize
    }),
    keepPreviousData: true,
  });

  // Reset page and accumulated trains when search term changes
  useEffect(() => {
    setPage(1);
    setAccumulatedTrains([]);
  }, [searchTerm]);

  // Update accumulated trains when data changes
  useEffect(() => {
    if (data?.items && data.items.length > 0) {
      if (page === 1) {
        // Reset accumulated trains for first page
        setAccumulatedTrains(data.items);
      } else {
        // Append new trains to accumulated trains, avoiding duplicates
        setAccumulatedTrains(prev => {
          const newTrains = data.items.filter(
            newTrain => !prev.some(existingTrain => existingTrain.id === newTrain.id)
          );
          return [...prev, ...newTrains];
        });
      }
    }
  }, [data, page]);

  // Load more trains when scrolling to the bottom
  const handleScroll = (e: React.UIEvent<HTMLDivElement>) => {
    const bottom = Math.abs(e.currentTarget.scrollHeight - e.currentTarget.scrollTop - e.currentTarget.clientHeight) < 1;
    if (bottom && data && data.pageNumber < data.totalPages && !isFetching) {
      setPage(prev => prev + 1);
    }
  };

  const handleSelect = (train: TrainDto) => {
    onSelect(train);
    setOpen(false);
  };

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-full justify-between"
          disabled={disabled}
        >
          Select a train
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[300px] p-0">
        <Command>
          <CommandInput 
            placeholder="Search trains..." 
            value={searchTerm}
            onValueChange={setSearchTerm}
          />
          <CommandList 
            className="max-h-[300px] overflow-auto" 
            onScroll={handleScroll}
            onWheel={(e) => {
              // Ensure wheel events are properly handled for scrolling
              e.currentTarget.scrollTop += e.deltaY;
              if (Math.abs(e.currentTarget.scrollHeight - e.currentTarget.scrollTop - e.currentTarget.clientHeight) < 1 && 
                  data && data.pageNumber < data.totalPages && !isFetching) {
                setPage(prev => prev + 1);
              }
            }}
          >
            {accumulatedTrains.length === 0 && !isLoading ? (
              <CommandEmpty>No trains found.</CommandEmpty>
            ) : null}
            <CommandGroup>
              {isLoading && page === 1 ? (
                <div className="p-2"><Loader /></div>
              ) : isError ? (
                <div className="p-2"><ErrorDisplay /></div>
              ) : accumulatedTrains.length === 0 && !isLoading ? (
                null
              ) : (
                accumulatedTrains.map((train) => (
                  <CommandItem
                    key={train.id}
                    value={train.id}
                    onSelect={() => handleSelect(train)}
                  >
                    <Check
                      className={cn(
                        "mr-2 h-4 w-4",
                        "opacity-0"
                      )}
                    />
                    {train.name}
                  </CommandItem>
                ))
              )}
              {isFetching && page > 1 && (
                <div className="p-2 text-center"><Loader /></div>
              )}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}