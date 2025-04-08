export type PaginatedResult<T extends object> = {
  items: T[],
  pageNumber: number,
  pageSize: number,
  totalCount: number,
  totalPages: number,
}