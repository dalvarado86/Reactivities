import React from 'react';
import { Header, Segment, Icon, Button } from 'semantic-ui-react';
import { Link } from "react-router-dom";

export default function NotFound() {
    return (
        <Segment placeholder>
            <Header icon>
                <Icon name='search'/>
                Oops! We've looked everywhere and could not find this.
            </Header>
            <Segment.Inline>
                <Button as={Link} to='/activities' primary>Return to activities page</Button>
            </Segment.Inline>
        </Segment>        
    )
}
