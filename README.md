## [Get this title for $10 on Packt's Spring Sale](https://www.packt.com/B12947?utm_source=github&utm_medium=packt-github-repo&utm_campaign=spring_10_dollar_2022)
-----
For a limited period, all eBooks and Videos are only $10. All the practical content you need \- by developers, for developers

# Mastering Azure Serverless Computing 

<a href="https://www.packtpub.com/cloud-networking/mastering-azure-serverless-computing?utm_source=github&utm_medium=repository&utm_campaign=9781789951226"><img src="https://www.packtpub.com/media/catalog/product/cache/e4d64343b1bc593f1c5348fe05efa4a6/9/7/9781789951226-original.jpeg" alt="Mastering Azure Serverless Computing " height="256px" align="right"></a>

This is the code repository for [Mastering Azure Serverless Computing ](https://www.packtpub.com/cloud-networking/mastering-azure-serverless-computing?utm_source=github&utm_medium=repository&utm_campaign=9781789951226), published by Packt.

**A practical guide to building and deploying enterprise-grade serverless applications using Azure Functions**

## What is this book about?
Application development has evolved from traditional monolithic app development to using serverless options and microservices. This book is designed to guide you through using Microsoft's Azure Functions to process data, integrate systems, and build simple APIs and microservices.


This book covers the following exciting features:
* Create and deploy advanced Azure Functions 
* Learn to extend the runtime of Azure Functions 
* Orchestrate your logic through code or a visual workflow 
* Add caching, security, routing, and filtering to your APIs 
* Use serverless technologies in real-world scenarios 

If you feel this book is for you, get your [copy](https://www.amazon.com/dp/1789951224) today!

<a href="https://www.packtpub.com/?utm_source=github&utm_medium=banner&utm_campaign=GitHubBanner"><img src="https://raw.githubusercontent.com/PacktPublishing/GitHub/master/GitHub.png" alt="https://www.packtpub.com/" border="5" /></a>

## Instructions and Navigations
All of the code is organized into folders. For example, Chapter02.

The code will look like the following:
```
public static class SimpleExample
{
[FunctionName("QueueTrigger")]
public static void Run(
[QueueTrigger("inputQueue")] string inItem,
[Queue("outputQueue")] out string outItem,
ILogger log)
{
log.LogInformation($"C# function processed: {inItem}");
}
}
```

**Following is what you need for this book:**
This book is designed for cloud administrators, architects, and developers interested in building scalable systems and deploying serverless applications with Azure Functions. Prior knowledge of core Microsoft Azure services and Azure Functions is necessary to understand the topics covered in this book.

With the following software and hardware list you can run all code files present in the book (Chapter 1-12).
### Software and Hardware List
| Chapter | Software required | OS required |
| -------- | ------------------------------------ | ----------------------------------- |
| 1,2,3,6,7 | Azure Functions Core Tool | Windows, Mac OS X, and Linux (Any) |
| 1,2,3,6,7 | Visual Studio 2019 Community | Windows, Mac OS X |
| 1,2,3 | Visual Studio Code | Windows, Mac OS X, and Linux (Any) |
| 7 | Docker Desktop | Windows, Mac OS X, and Linux (Any) |

We also provide a PDF file that has color images of the screenshots/diagrams used in this book. [Click here to download it](https://static.packt-cdn.com/downloads/9781789951226_ColorImages.pdf).

### Related products
* Azure Serverless Computing Cookbook - Second Edition  [[Packt]](https://www.packtpub.com/virtualization-and-cloud/azure-serverless-computing-cookbook-second-edition?utm_source=github&utm_medium=repository&utm_campaign=9781789615265) [[Amazon]](https://www.amazon.com/dp/1789615267)

## Get to Know the Author
**Lorenzo Barbieri**
specializes in cloud-native applications and application modernization on Azure and Office 365, Windows and cross-platform applications, Visual Studio, and DevOps, and likes to talk with people and communities about technology, food, and funny things.
He is a speaker, a trainer, and a public speaking coach. He has helped many students, developers, and other professionals, as well as many of his colleagues, to improve their stage presence with a view to delivering exceptional presentations.
Lorenzo works for Microsoft, in the One Commercial Partner Technical Organization, helping partners, developers, communities, and customers across Western Europe, supporting software development on Microsoft and OSS technologies.

**Massimo Bonanni**
specializes in cloud application development and, in particular, in Azure compute technologies. Over the last 3 years, he has worked with important Italian and European customers to implement distributed applications using Service Fabric and microservices architecture.
Massimo is an Azure technical trainer in Microsoft and his goal is to help customers utilize their Azure skills to achieve more and leverage the power of Azure in their solutions. He is also a technical speaker at national and international conferences, a Microsoft Certified Trainer, a former MVP (for 6 years in Visual Studio and Development Technologies and Windows Development), an Intel Software Innovator, and an Intel Black Belt.

### Suggestions and Feedback
[Click here](https://docs.google.com/forms/d/e/1FAIpQLSdy7dATC6QmEL81FIUuymZ0Wy9vH1jHkvpY57OiMeKGqib_Ow/viewform) if you have any feedback or suggestions.


