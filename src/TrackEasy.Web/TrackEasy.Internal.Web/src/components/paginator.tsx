import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination.tsx";

export type PaginatorProps = {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
};

const DOTS = '...';

function getPaginationRange(
  totalPages: number,
  currentPage: number,
  siblingCount: number = 1
): (number | string)[] {
  const totalPageNumbers = siblingCount * 2 + 5;

  if (totalPages <= totalPageNumbers) {
    return Array.from({length: totalPages}, (_, i) => i + 1);
  }

  const leftSiblingIndex = Math.max(currentPage - siblingCount, 1);
  const rightSiblingIndex = Math.min(currentPage + siblingCount, totalPages);

  const shouldShowLeftDots = leftSiblingIndex > 2;
  const shouldShowRightDots = rightSiblingIndex < totalPages - 1;
  const firstPageIndex = 1;
  const lastPageIndex = totalPages;

  if (!shouldShowLeftDots && shouldShowRightDots) {
    const leftItemCount = 3 + 2 * siblingCount;
    const leftRange = Array.from({length: leftItemCount}, (_, i) => i + 1);
    return [...leftRange, DOTS, lastPageIndex];
  }

  if (shouldShowLeftDots && !shouldShowRightDots) {
    const rightItemCount = 3 + 2 * siblingCount;
    const rightRange = Array.from(
      {length: rightItemCount},
      (_, i) => totalPages - rightItemCount + 1 + i
    );
    return [firstPageIndex, DOTS, ...rightRange];
  }

  if (shouldShowLeftDots && shouldShowRightDots) {
    const middleRange = Array.from(
      {length: rightSiblingIndex - leftSiblingIndex + 1},
      (_, i) => leftSiblingIndex + i
    );
    return [firstPageIndex, DOTS, ...middleRange, DOTS, lastPageIndex];
  }

  return [];
}

export function Paginator(props: PaginatorProps) {
  const {currentPage, totalPages, onPageChange} = props;

  const paginationRange = getPaginationRange(totalPages, currentPage, 1);

  return (
    <Pagination>
      <PaginationContent>
        <PaginationItem>
          <PaginationPrevious
            aria-disabled={currentPage === 1}
            onClick={() => {
              if (currentPage > 1) onPageChange(currentPage - 1);
            }}
          />
        </PaginationItem>

        {paginationRange.map((item, index) => {
          if (typeof item === "number") {
            return (
              <PaginationItem
                key={index}
                onClick={() => onPageChange(item)}
              >
                <PaginationLink isActive={item === currentPage}>{item}</PaginationLink>
              </PaginationItem>
            );
          }
          return <PaginationEllipsis key={index}/>;
        })}

        <PaginationItem>
          <PaginationNext
            aria-disabled={currentPage === totalPages}
            onClick={() => {
              if (currentPage < totalPages) onPageChange(currentPage + 1);
            }}
          />
        </PaginationItem>
      </PaginationContent>
    </Pagination>
  );
}