import {ReactNode, useEffect} from "react";
import {useNavigate} from "react-router-dom";
import {useAuthStore} from "@/stores/auth-store.ts";
import {useUserStore} from "@/stores/user-store.ts";
import {toast} from "sonner";

interface ProtectedRouteProps {
  children: ReactNode;
  requiredRoles?: string[];
}

export default function ProtectedRoute({children, requiredRoles}: ProtectedRouteProps) {
  const navigate = useNavigate();
  const {checkAuth, needsTwoFactor} = useAuthStore();
  const {user} = useUserStore();

  useEffect(() => {
    if (!checkAuth()) {
      navigate("/login");
      return;
    }

    if (needsTwoFactor) {
      navigate("/two-factor");
      return;
    }

    if (requiredRoles && requiredRoles.length > 0 && user) {
      if (!requiredRoles.includes(user.role)) {
        toast.error("You don't have permission to access this page");
        navigate("/");
        return;
      }
    }
  }, [checkAuth, navigate, needsTwoFactor, requiredRoles, user]);

  return <>{children}</>;
}
