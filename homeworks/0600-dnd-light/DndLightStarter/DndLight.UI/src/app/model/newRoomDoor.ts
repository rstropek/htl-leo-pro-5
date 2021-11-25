import { ItemType } from './itemType';

export interface NewRoomDoor { 
    linkedRoomId: number;
    description: string;
    initiallyLocked: boolean;
    requiredItemToUnlock?: ItemType;
}