import { NewRoomDoor } from './newRoomDoor';
import { NewRoomItem } from './newRoomItem';
import { NewRoomMonster } from './newRoomMonster';

export interface NewRoomDetails { 
    description: string;
    doors: Array<NewRoomDoor>;
    items: Array<NewRoomItem>;
    monsters: Array<NewRoomMonster>;
}