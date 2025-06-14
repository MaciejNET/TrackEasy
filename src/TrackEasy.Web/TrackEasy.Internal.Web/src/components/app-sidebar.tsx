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

  
  const filteredItems = sidebarItems.filter(item => {
    
    if (!item.requiredRoles || item.requiredRoles.length === 0) {
      return true;
    }

    
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
