import {Loader2} from "lucide-react";

export function Loader({size}: { size?: number }) {
  size ??= 35

  return (
    <div className="flex justify-center py-2">
      <Loader2 className="animate-spin" size={size}/>
    </div>
  )
}