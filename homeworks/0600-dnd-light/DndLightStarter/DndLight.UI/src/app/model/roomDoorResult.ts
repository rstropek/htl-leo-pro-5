import { ItemType } from './itemType';

export interface RoomDoorResult { 
    id: number;
    linkedRoomId: number;
    description: string;
    initiallyLocked: boolean;
    requiredItemToUnlock?: ItemType;
}