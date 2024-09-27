import React, { useState } from 'react';
import Navbar from '../../components/Navbar';
import AboutComponent from '../../components/about/about';
import ProfileComponent from '../../components/profile/profile';
// Import other components used in the home page
import ProjectComponent from '../../components/project/project';
import ServiceComponent from '../../components/service/service';
import TeamComponent from '../../components/team/team'; 


const UserHomePage = () => {
    const [isLoggedIn, setIsLoggedIn] = useState(true); // Assuming user is logged in
    const userName = "User"; // Replace with actual user name

    return (
        <div>
            <Navbar isLoggedIn={isLoggedIn} userName={userName} />
            <AboutComponent isLoggedIn={isLoggedIn} />
            <ProfileComponent isLoggedIn={isLoggedIn} />
            <ProjectComponent isLoggedIn={isLoggedIn} />
            <ServiceComponent isLoggedIn={isLoggedIn} />
            <TeamComponent isLoggedIn={isLoggedIn} />
            {/* Add other components used in the home page */}
        </div>
    );
};

export default UserHomePage;