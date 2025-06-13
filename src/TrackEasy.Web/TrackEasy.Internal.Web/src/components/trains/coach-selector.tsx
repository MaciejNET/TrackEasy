import {useState, useEffect, useRef} from "react";
import {useQuery} from "@tanstack/react-query";
import {fetchCoaches} from "@/api/coaches-api.ts";
import {CoachDto} from "@/schemas/coach-schema.ts";
import {Input} from "@/components/ui/input.tsx";
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

type CoachSelectorProps = {
  operatorId: string;
  onSelect: (coach: CoachDto) => void;
  disabled?: boolean;
};

export function CoachSelector(props: CoachSelectorProps) {
  const {operatorId, onSelect, disabled} = props;
  const [open, setOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [page, setPage] = useState(1);
  const [accumulatedCoaches, setAccumulatedCoaches] = useState<CoachDto[]>([]);
  const pageSize = 10;

  const {
    data,
    isLoading,
    isError,
    isFetching
  } = useQuery({
    queryKey: ['coaches-for-selector', operatorId, searchTerm, page],
    queryFn: () => fetchCoaches(operatorId, {
      code: searchTerm,
      pageNumber: page,
      pageSize: pageSize
    }),
    keepPreviousData: true,
  });

  // Reset page and accumulated coaches when search term changes
  useEffect(() => {
    setPage(1);
    setAccumulatedCoaches([]);
  }, [searchTerm]);

  // Update accumulated coaches when data changes
  useEffect(() => {
    if (data?.items && data.items.length > 0) {
      if (page === 1) {
        // Reset accumulated coaches for first page
        setAccumulatedCoaches(data.items);
      } else {
        // Append new coaches to accumulated coaches, avoiding duplicates
        setAccumulatedCoaches(prev => {
          const newCoaches = data.items.filter(
            newCoach => !prev.some(existingCoach => existingCoach.id === newCoach.id)
          );
          return [...prev, ...newCoaches];
        });
      }
    }
  }, [data, page]);

  // Load more coaches when scrolling to the bottom
  const handleScroll = (e: React.UIEvent<HTMLDivElement>) => {
    const bottom = Math.abs(e.currentTarget.scrollHeight - e.currentTarget.scrollTop - e.currentTarget.clientHeight) < 1;
    if (bottom && data && data.pageNumber < data.totalPages && !isFetching) {
      setPage(prev => prev + 1);
    }
  };

  const handleSelect = (coach: CoachDto) => {
    onSelect(coach);
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
          Select a coach
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[300px] p-0">
        <Command>
          <CommandInput 
            placeholder="Search coaches..." 
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
            {accumulatedCoaches.length === 0 && !isLoading ? (
              <CommandEmpty>No coaches found.</CommandEmpty>
            ) : null}
            <CommandGroup>
              {isLoading && page === 1 ? (
                <div className="p-2"><Loader /></div>
              ) : isError ? (
                <div className="p-2"><ErrorDisplay /></div>
              ) : accumulatedCoaches.length === 0 && !isLoading ? (
                null
              ) : (
                accumulatedCoaches.map((coach) => (
                  <CommandItem
                    key={coach.id}
                    value={coach.id}
                    onSelect={() => handleSelect(coach)}
                  >
                    <Check
                      className={cn(
                        "mr-2 h-4 w-4",
                        "opacity-0"
                      )}
                    />
                    {coach.code}
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
