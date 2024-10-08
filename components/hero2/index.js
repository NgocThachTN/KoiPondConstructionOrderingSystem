import React from "react";
import Slider from "react-slick";
import Link from 'next/link'



const Hero2 = (props) => {

    var settings = {
        dots: false,
        arrows: true,
        speed: 1200,
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2500,
        fade: true
    };

    return (
        <section className="wpo-box-style">
            <div className={`wpo-hero-slider wpo-hero-style-2 ${props.heroClass}`}>
                <div className="hero-container">
                    <div className="hero-wrapper">
                        <Slider {...settings}>
                            <div className="hero-slide">
                                <div className="slide-inner" style={{ backgroundImage: `url(${'/images/slider/slide-1.jpg'})` }}>
                                    <div className="container-fluid">
                                        <div className="slide-content">
                                            <div className="slide-title">
                                                <h2>Koi Pond Construction</h2>
                                            </div>
                                            <div className="slide-title-sub">
                                                <p>Creating unforgettable disruptions in perfect harmony.</p>
                                            </div>
                                            <div className="clearfix"></div>
                                            <div className="slide-btns">
                                                <Link href="/about" className="theme-btn">Discover More</Link>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="hero-slide">
                                <div className="slide-inner" style={{ backgroundImage: `url(${'/images/slider/slide-2.jpg'})` }}>
                                    <div className="container-fluid">
                                        <div className="slide-content">
                                            <div className="slide-title">
                                                <h2>Making Waves, Breaking Peace</h2>
                                            </div>
                                            <div className="slide-title-sub">
                                                <p>Redefining calm by introducing chaos.</p>
                                            </div>
                                            <div className="clearfix"></div>
                                            <div className="slide-btns">
                                                <Link href="/about" className="theme-btn">Discover More</Link>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="hero-slide">
                                <div className="slide-inner" style={{ backgroundImage: `url(${'/images/slider/slide-3.jpg'})` }}>
                                    <div className="container-fluid">
                                        <div className="slide-content">
                                            <div className="slide-title">
                                                <h2>From Tranquility to Chaos</h2>
                                            </div>
                                            <div className="slide-title-sub">
                                                <p>Because still waters were made to be stirred.</p>
                                            </div>
                                            <div className="clearfix"></div>
                                            <div className="slide-btns">
                                                <Link href="/about" className="theme-btn">Discover More</Link>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </Slider>
                    </div>
                </div>
            </div>
        </section>
    )
}

export default Hero2;