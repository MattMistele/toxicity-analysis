# What is this project?

I worked on this project over my 2018/2019 winter break to dip my toes into the world of machine learning.

Using Wikipedia's open source comment toxicity training data, I created a ML model that can predict whether a comment is toxic or not (likely to make people want to leave the discussion).

The model will identify whether a comment is toxic, and assign it a probability as a precentage from 0 - 100. Where 0 is certainly non-toxic and 100 is certainly toxic.

I then used the model to analyse various youtube channels to see what precentage of the comments they recieve are toxic.  
I scraped and analysed 10,00 comments from:
* [The 2018 Youtube Rewind](https://www.youtube.com/watch?v=YbJOTdZBX1g), which is currently the most disliked video on YouTube
* [The WSJ Youtube Channel](https://www.youtube.com/user/WSJDigitalNetwork)
* [Vsauce](https://www.youtube.com/user/Vsauce), a channel on science topics
* [Khan Academy](https://www.youtube.com/user/khanacademy), which makes instructional videos for school

# Results

The toxic comments from the analysis and their probability can all be found in the "results" folder.  
Here are the precentage of toxic comments for each:
* 2.17% of the [2018 Youtube Rewind's](https://www.youtube.com/watch?v=YbJOTdZBX1g) comments are toxic
* 4.28% of [The WSJ Youtube Channel's](https://www.youtube.com/user/WSJDigitalNetwork) comments are toxic
* 6.39% of [Vsauce's](https://www.youtube.com/user/Vsauce) comments are toxic
* 0.00% of [Khan Academy's](https://www.youtube.com/user/khanacademy) comments are toxic

After running these tests, I was a little confused. Shouldn't the most disliked video on YouTube have the most toxic comments? Why does the science channel foster so much toxicity?

After scrolling through a lot of the comments, here's my theory:  
**Disagreement among users causes toxicity.**

* Everyone agrees that they dislike the 2018 YouTube rewind. Sure, there are comments that are toxic towards the video itself, but most of the comments are about agreeing and supporting each other.
* While the WSJ is generally considered a professional news source, they don't control who comments on their videos. A lot of the videos cover politics, which is very divisive, especially among internet users.
* I found that a lot of the toxicity from Vsauce came from comment threads. There were very long threads of users arguing among each other about how particular science things work, thus causing toxicity
* Khan Academy is one of the few places to go that doesn't harbour toxicity. They don't recieve a lot of comments besides the the few "Thank you for this video" type.

# Conclusion

In almost every YouTube comment section you will find toxicity; the platform is notorious for that.  

Nonetheless, I have thoroughly enjoyed learning more about ML and creating this model to analyse comments. It's amazing that this model can pick up on comments you wouldn't think a computer would, such as really hateful comments that don't even use swear words, or comments that use hateful language spelled wrong or that you wouldn't find in a dictionary, such as:  
* "Well black people have low IQ?"  
* "HEYYYYYYYYYYYYYY W SAUCEEEEEEEEEEEE MICHEAAAAAAALLLLLLLL HEEEEREEEEEE!!!!!!!!!?"   
* "POtAtO QuEEn cus it's GAYYYYY?"  

# Works Cited

* To learn ML.NET, I followed Microsoft's binary classification tutorial  
https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/sentiment-analysis

* The Wikipedia dataset is open source here:  
  https://meta.wikimedia.org/wiki/Research:Detox/Data_Release

* To transform those annotated comments into a form that the model could process, I used Nick Bitounis' gist here:  
  https://gist.github.com/nickntg/1ce67727d3047146ffe0f8192aba6177
