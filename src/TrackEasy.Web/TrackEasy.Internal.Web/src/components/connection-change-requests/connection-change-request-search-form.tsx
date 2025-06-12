import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card.tsx";
import {useConnectionChangeRequestStore} from "@/stores/connection-change-request-store.ts";

export function ConnectionChangeRequestSearchForm() {
  const {resetSearchParams} = useConnectionChangeRequestStore();

  return (
    <Card className="mb-6">
      <CardHeader>
        <CardTitle>Connection Change Requests</CardTitle>
        <CardDescription>
          Review and manage connection change requests from operators
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="flex justify-end">
          <button
            onClick={() => resetSearchParams()}
            className="px-4 py-2 text-sm font-medium text-gray-700 bg-gray-100 rounded-md hover:bg-gray-200"
          >
            Reset Filters
          </button>
        </div>
      </CardContent>
    </Card>
  );
}