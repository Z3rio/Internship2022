import "./NavBar.css";

import { useState, MouseEvent } from "react";

import {
  AppBar,
  Toolbar,
  IconButton,
  Button,
  ButtonGroup,
  Typography,
  Menu,
  MenuItem,
} from "@mui/material";

interface PropsStruct {
  loggedIn: boolean;
  setLoggedIn: Function;
}

import Icon from "@mdi/react";
import { mdiFoodApple, mdiAccount } from "@mdi/js";

export default function NavBar(props: PropsStruct) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const handleMenu = (event: MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  function handleMenuClick(action: string) {}

  return (
    <AppBar position="static">
      <Toolbar>
        <IconButton size="large" edge="start" aria-label="menu" sx={{ mr: 2 }}>
          <Icon size={1.25} path={mdiFoodApple} />
        </IconButton>

        <Typography component="div" className="header">
          Food searcher
        </Typography>

        {props.loggedIn ? (
          <div className="user-menu">
            <IconButton
              size="large"
              edge="start"
              color="primary"
              aria-label="menu"
              onClick={handleMenu}
              sx={{ mr: 2 }}
            >
              <Icon size={1.25} path={mdiAccount} />
            </IconButton>
            <Menu
              anchorEl={anchorEl}
              keepMounted
              open={Boolean(anchorEl)}
              onClose={handleClose}
            >
              <MenuItem
                onClick={() => {
                  handleMenuClick("profile");
                }}
              >
                My profile
              </MenuItem>
              <MenuItem>Favorite Resturants</MenuItem>
              <MenuItem>Settings</MenuItem>
              <MenuItem>Log out</MenuItem>
            </Menu>
          </div>
        ) : (
          <ButtonGroup>
            <Button color="primary">Sign up</Button>
            <Button color="primary">Log In</Button>
          </ButtonGroup>
        )}
      </Toolbar>
    </AppBar>
  );
}
