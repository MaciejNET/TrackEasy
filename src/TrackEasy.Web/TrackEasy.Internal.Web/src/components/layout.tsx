import {SidebarProvider} from "@/components/ui/sidebar.tsx";
import {AppSidebar} from "@/components/app-sidebar.tsx";
import {Outlet, useNavigate} from "react-router-dom";
import {Button} from "@/components/ui/button.tsx";
import {LogOut, Loader2, KeyRound} from "lucide-react";
import {ThemeProvider} from "@/components/theme-provider.tsx";
import {ModeToggle} from "@/components/mode-toggler.tsx";
import {Toaster} from "@/components/ui/sonner.tsx";
import {useUserStore} from "@/stores/user-store.ts";
import {useAuthStore} from "@/stores/auth-store.ts";
import {useEffect, useState} from "react";
import {ChangePasswordModal} from "@/components/change-password-modal.tsx";
import {NotificationsPanel} from "@/components/notifications-panel.tsx";

export default function Layout() {
  const navigate = useNavigate();
  const {user, fetchUser, isLoading} = useUserStore();
  const {logout} = useAuthStore();
  const [isChangePasswordModalOpen, setIsChangePasswordModalOpen] = useState(false);

  useEffect(() => {
    // Fetch user data when component mounts
    fetchUser();
  }, [fetchUser]);

  const handleLogout = async () => {
    await logout();
    useUserStore.getState().clearUser(); // Clear user data when logging out
    navigate("/login");
  };

  return (
    <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
      <div className="flex h-screen">
        <SidebarProvider>
          <AppSidebar/>

          <div className="flex flex-col flex-1 overflow-auto">
            <header className="flex justify-between items-center border-b bg-secondary px-4 py-2">
              <h1 className="font-semibold text-xl">Dashboard</h1>
              <div className="flex items-center gap-3">
                <NotificationsPanel />
                <ModeToggle/>
                <span className="text-sm">
                  {isLoading ? (
                    <Loader2 className="h-4 w-4 animate-spin" />
                  ) : user ? (
                    `${user.firstName} ${user.lastName}`
                  ) : (
                    "User"
                  )}
                </span>
                <Button 
                  variant="outline" 
                  className="cursor-pointer"
                  onClick={() => setIsChangePasswordModalOpen(true)}
                >
                  <KeyRound className="mr-2 h-4 w-4"/> Change Password
                </Button>
                <Button 
                  variant="outline" 
                  className="cursor-pointer"
                  onClick={handleLogout}
                >
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

      {/* Change Password Modal */}
      <ChangePasswordModal 
        open={isChangePasswordModalOpen} 
        setOpen={setIsChangePasswordModalOpen} 
      />
    </ThemeProvider>
  );
}
