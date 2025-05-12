import {ElementType} from "react";
import { HomeIcon, TicketPercent, Building2 } from "lucide-react";
import { Roles } from "@/lib/roles";

export type SidebarItem = {
  Title: string;
  Icon: ElementType;
  Url: string;
  requiredRoles?: string[];
}

export const sidebarItems: SidebarItem[] = [
  {
    Title: "Home",
    Icon: HomeIcon,
    Url: "/"
    // No requiredRoles means all authenticated users can access
  },
  {
    Title: "Discounts",
    Icon: TicketPercent,
    Url: "/discounts",
    requiredRoles: [Roles.Admin]
  },
  {
    Title: "Cities",
    Icon: Building2,
    Url: "/cities",
    requiredRoles: [Roles.Admin]
  }
]
