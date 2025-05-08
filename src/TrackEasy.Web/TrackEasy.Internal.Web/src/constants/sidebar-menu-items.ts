import {ElementType} from "react";
import { HomeIcon, TicketPercent, Building2 } from "lucide-react";

export type SidebarItem = {
  Title: string;
  Icon: ElementType;
  Url: string;
}

export const sidebarItems: SidebarItem[] = [
  {
    Title: "Home",
    Icon: HomeIcon,
    Url: "/"
  },
  {
    Title: "Discounts",
    Icon: TicketPercent,
    Url: "/discounts"
  },
  {
    Title: "Cities",
    Icon: Building2,
    Url: "/cities"
  }
]
