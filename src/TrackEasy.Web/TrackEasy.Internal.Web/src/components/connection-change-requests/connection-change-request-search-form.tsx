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
      </CardContent>
    </Card>
  );
}
