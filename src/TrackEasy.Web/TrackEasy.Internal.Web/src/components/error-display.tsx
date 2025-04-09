import {AlertCircle} from "lucide-react"

import {Card, CardContent} from "@/components/ui/card"

export function ErrorDisplay() {
  return (
    <Card className="w-full border-dashed border-destructive/50">
      <CardContent className="flex flex-col items-center justify-center py-10 text-center">
        <div className="mb-4 rounded-full bg-destructive/10 p-3">
          <AlertCircle className="h-10 w-10 text-destructive"/>
        </div>
        <h3 className="mb-2 text-lg font-semibold text-destructive">Error occurred</h3>
        <p className="text-sm text-muted-foreground">Unable to load data. Please try again later.</p>
      </CardContent>
    </Card>
  )
}
