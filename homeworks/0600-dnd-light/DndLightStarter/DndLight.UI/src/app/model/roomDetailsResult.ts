import { RoomDoorResult } from './roomDoorResult';
import { RoomItemResult } from './roomItemResult';
import { RoomMonsterResult } from './roomMonsterResult';

export interface RoomDetailsResult { 
    id: number;
    description: string;
    doors: Array<RoomDoorResult>;
    items: Array<RoomItemResult>;
    monsters: Array<RoomMonsterResult>;
}