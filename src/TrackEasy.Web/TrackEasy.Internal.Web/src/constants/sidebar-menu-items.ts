import {ElementType} from "react";
import { HomeIcon, TicketPercent } from "lucide-react";

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
  }
]