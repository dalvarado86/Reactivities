import { createContext, useContext } from 'react';
import ActivityStre from './activityStore';
import CommonStore from './commonStore';
import UserStore from './userStore';
import ModalStore from './modalStore';

interface Store {
    activityStore: ActivityStre;
    commonStore: CommonStore;
    userStore: UserStore;
    modalStore: ModalStore;
}

export const store: Store = {
    activityStore: new ActivityStre(),
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}