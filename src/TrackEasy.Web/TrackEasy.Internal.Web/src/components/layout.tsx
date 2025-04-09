import {SidebarProvider} from "@/components/ui/sidebar.tsx";
import {AppSidebar} from "@/components/app-sidebar.tsx";
import {Outlet} from "react-router-dom";
import {Button} from "@/components/ui/button.tsx";
import {LogOut} from "lucide-react";
import {ThemeProvider} from "@/components/theme-provider.tsx";
import {ModeToggle} from "@/components/mode-toggler.tsx";
import {Toaster} from "@/components/ui/sonner.tsx";

export default function Layout() {
  return (
    <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
      <div className="flex h-screen">
        <SidebarProvider>
          <AppSidebar/>

          <div className="flex flex-col flex-1 overflow-auto">
            <header className="flex justify-between items-center border-b bg-secondary px-4 py-2">
              <h1 className="font-semibold text-xl">Dashboard</h1>
              <div className="flex items-center gap-3">
                <ModeToggle/>
                <span className="text-sm">Username</span>
                <Button variant="outline" className="cursor-pointer">
                  <LogOut className="mr-2 h-4 w-4"/> Logout
                </Button>
              </div>
            </header>

            <main className="flex-1 p-4 overflow-auto">
              <Outlet/>
              <Toaster richColors/>
            </main>
          </div>
        </SidebarProvider>
      </div>
    </ThemeProvider>
  );
}
