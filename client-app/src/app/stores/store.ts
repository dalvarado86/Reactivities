import { createContext, useContext } from 'react';
import ActivityStre from './activityStore';

interface Store {
    activityStore: ActivityStre
}

export const store: Store = {
    activityStore: new ActivityStre()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}