import {FileX} from "lucide-react"

import {Card, CardContent} from "@/components/ui/card"

export function NoData() {
  return (
    <Card className="w-full border-dashed">
      <CardContent className="flex flex-col items-center justify-center py-10 text-center">
        <div className="mb-4 rounded-full bg-muted p-3">
          <FileX className="h-10 w-10 text-muted-foreground"/>
        </div>
        <h3 className="mb-2 text-lg font-semibold">No data available</h3>
        <p className="text-sm text-muted-foreground">There are no items to display.</p>
      </CardContent>
    </Card>
  )
}
