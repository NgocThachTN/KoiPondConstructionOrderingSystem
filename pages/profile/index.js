import React from 'react';
import Header from '../../components/Header/Header';
import Footer from '../../components/Footer/Footer';

const Profile = () => {
    return (
        <div>
            <Header hclass="header-class" Logo="/path/to/logo.png" />
            <section className="profile-section">
                <div className="container">
                    <div className="row">
                        <div className="col-lg-12">
                            <div className="profile-content">
                                <h1>Profile Page</h1>
                                <p>Welcome to your profile page. Here you can view and edit your personal information.</p>
                                <div className="profile-details">
                                    <h2>Personal Information</h2>
                                    <form>
                                        <div className="form-group">
                                            <label htmlFor="name">Name:</label>
                                            <input type="text" id="name" name="name" className="form-control" />
                                        </div>
                                        <div className="form-group">
                                            <label htmlFor="email">Email:</label>
                                            <input type="email" id="email" name="email" className="form-control" />
                                        </div>
                                        <div className="form-group">
                                            <label htmlFor="password">Password:</label>
                                            <input type="password" id="password" name="password" className="form-control" />
                                        </div>
                                        <button type="submit" className="btn btn-primary">Save Changes</button>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <Footer />
        </div>
    );
};

export default Profile;
