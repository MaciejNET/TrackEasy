import { Button } from "@/components/ui/button";
import { Train, LogIn, UserPlus } from "lucide-react";

export function Header() {
  return (
    <div className="bg-white/80 backdrop-blur-sm border-b border-gray-200/50">
      <div className="max-w-7xl mx-auto px-6 py-4">
        <div className="flex items-center justify-between">
          {/* Logo and Brand */}
          <div className="flex items-center gap-3">
            <div className="p-2 bg-blue-600 rounded-lg">
              <Train className="h-6 w-6 text-white" />
            </div>
            <h1 className="text-2xl font-bold text-gray-900">TrackEasy</h1>
          </div>

          {/* Auth Buttons */}
          <div className="flex items-center gap-3">
            <Button variant="outline" size="sm" className="flex items-center gap-2">
              <LogIn className="h-4 w-4" />
              Login
            </Button>
            <Button size="sm" className="flex items-center gap-2 bg-blue-600 hover:bg-blue-700">
              <UserPlus className="h-4 w-4" />
              Register
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}
