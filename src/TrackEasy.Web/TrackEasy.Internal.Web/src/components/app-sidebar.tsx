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

export function AppSidebar() {
  const location = useLocation();

  return (
    <Sidebar>
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupContent>
            {sidebarItems.map((item: SidebarItem) => {
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