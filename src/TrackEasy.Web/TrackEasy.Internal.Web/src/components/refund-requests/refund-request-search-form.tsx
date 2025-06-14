import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card.tsx";
import {useRefundRequestStore} from "@/stores/refund-request-store.ts";
import {useEffect} from "react";
import {Button} from "@/components/ui/button.tsx";
import {useUserStore} from "@/stores/user-store.ts";

export function RefundRequestSearchForm() {
  const {searchParams, setSearchParams, resetSearchParams} = useRefundRequestStore();
  const {user} = useUserStore();

  
  useEffect(() => {
    if (user?.operatorId) {
      setSearchParams({operatorId: user.operatorId, pageNumber: 1});
    }
  }, [user, setSearchParams]);

  const handleReset = () => {
    if (user?.operatorId) {
      resetSearchParams(user.operatorId);
    }
  };

  return (
    <Card className="mb-6">
      <CardHeader>
        <CardTitle>Refund Requests</CardTitle>
        <CardDescription>
          Review and manage refund requests from passengers
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="flex justify-end">
          <Button
            onClick={handleReset}
            variant="outline"
            disabled={!user?.operatorId}
          >
            Reset Filters
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}
