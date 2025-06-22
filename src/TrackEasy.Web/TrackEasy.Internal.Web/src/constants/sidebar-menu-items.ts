import {ElementType} from "react";
import { HomeIcon, TicketPercent, Building2, Train, Tag, Users, UserCog, Bus, FileText, RefreshCcw, Map } from "lucide-react";
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
    
  },
  {
    Title: "Coaches",
    Icon: Bus,
    Url: "/coaches",
    requiredRoles: [Roles.Manager]
  },
  {
    Title: "Trains",
    Icon: Train,
    Url: "/trains",
    requiredRoles: [Roles.Manager]
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
  },
  {
    Title: "Stations",
    Icon: Train,
    Url: "/stations",
    requiredRoles: [Roles.Admin]
  },
  {
    Title: "Discount Codes",
    Icon: Tag,
    Url: "/discount-codes",
    requiredRoles: [Roles.Admin]
  },
  {
    Title: "Operators",
    Icon: Users,
    Url: "/operators",
    requiredRoles: [Roles.Admin]
  },
  {
    Title: "User Management",
    Icon: UserCog,
    Url: "/user-management",
    requiredRoles: [Roles.Admin]
  },
  {
    Title: "Connection Change Requests",
    Icon: FileText,
    Url: "/connection-change-requests",
    requiredRoles: [Roles.Admin]
  },
  {
    Title: "Refund Requests",
    Icon: RefreshCcw,
    Url: "/refund-requests",
    requiredRoles: [Roles.Manager]
  },
  {
    Title: "Connections",
    Icon: Map,
    Url: "/connections",
    requiredRoles: [Roles.Manager]
  }
]
