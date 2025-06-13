import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarGroupContent,
  SidebarMenuButton,
  SidebarMenuItem
} from "@/components/ui/sidebar.tsx";
import {Link, useLocation} from "react-router-dom";
import {SidebarItem, sidebarItems} from "@/constants/sidebar-menu-items.ts";
import {useUserStore} from "@/stores/user-store.ts";

export function AppSidebar() {
  const location = useLocation();
  const { user } = useUserStore();

  // Filter sidebar items based on user role
  const filteredItems = sidebarItems.filter(item => {
    // If no required roles are specified, show the item to all authenticated users
    if (!item.requiredRoles || item.requiredRoles.length === 0) {
      return true;
    }

    // If user has no role or the item requires roles that don't include the user's role, hide the item
    return user && item.requiredRoles.includes(user.role);
  });

  return (
    <Sidebar>
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupContent>
            {filteredItems.map((item: SidebarItem) => {
              const isActive = location.pathname === item.Url;
              return (
                <SidebarMenuItem key={item.Title} className="list-none">
                  <SidebarMenuButton asChild size="lg" isActive={isActive}>
                    <Link to={item.Url}>
                      <item.Icon/>
                      <span>{item.Title}</span>
                    </Link>
                  </SidebarMenuButton>
                </SidebarMenuItem>
              );
            })}
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>
    </Sidebar>
  )
}
